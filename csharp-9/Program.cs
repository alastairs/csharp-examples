using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace csharp_9
{
    // Short-hand record declaration:
    // public record User(string Username, string Name);

    // Medium-hand record declaration:
    // public record User
    // {
    //     string Username;
    //     string Name;
    // }

    // And equivalently:
    // public record User
    // {
    //     string Username { get; }
    //     string Name { get; }
    // }

    // MUTABLE record declaration:
    // public record User
    // {
    //     string? Username { get; set; }
    //     string? Name { get; set; }
    // }

    public class Program
    {
        public Program()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Uri = "https://api.github.com/";
        }

        // init-only accessor
        public string? Uri { get; init; }

        public static async Task Main(string[] args)
        {
            // Target-typed new
            Program program = new();
            await program.RunAsync("https://api.github.com/");
        }

        public async Task<bool> RunAsync(string uri)
        {
            // Attributes on local functions
            static async Task<(Exception? ex, HttpResponseMessage? res)> SendRequest([NotNull] string url)
            {
                url ??= "https://api.github.com/";

                Console.WriteLine("Calling GitHub 🐙🐱");
                try
                {
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.UserAgent.ParseAdd($"Mozilla/5.0 (compatible; github.com/alastairs/csharp-examples)");
                    var response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();
                    return (null, response);
                }
                catch (Exception ex) when (ex is HttpRequestException)
                {
                    return (ex, null);
                }
            }

            var result = await SendRequest(uri);
            return result switch
            {
                // Type patterns finally give us a slick Either<TError, TResult> implementation
                (Exception, null) => WriteError(result.ex),
                (null, HttpResponseMessage) => await WriteResponse(result.res),
                _ => throw new InvalidOperationException()
            };
        }

        private static async Task<bool> WriteResponse(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStreamAsync();
            var parsedJson = await JsonSerializer.DeserializeAsync<Responses.IndexResponse>(responseBody, JsonOptions);

            // using declarations - calls Dispose() when the variable goes out of scope
            using var stdout = Console.OpenStandardOutput();
            await JsonSerializer.SerializeAsync(stdout, parsedJson, JsonOptions);

            return true;
        }

        private static bool WriteError(Exception ex)
        {
            Console.WriteLine($"The request failed 😞 {ex.Message}");
            return false;
        }

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            DictionaryKeyPolicy = new SnakeCaseNamingPolicy(),
            WriteIndented = true
        };
    }
}
