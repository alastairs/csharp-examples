using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace csharp_8
{
    public class Program
    {
        public Program() => Console.OutputEncoding = Encoding.UTF8;

        public static async Task Main(string[] args)
        {
            await new Program().RunAsync("https://api.github.com/");
        }

        public async Task<bool> RunAsync([NotNull] string uri)
        {
            // static local function
            static async Task<(Exception? ex, HttpResponseMessage? res)> SendRequest(string url)
            {
                // Null-coalescing assignment
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

            // Improved pattern matching, with switch expressions, for a much more elegant
            // Either<Error, Result> implementation
            var response = await SendRequest(uri);
            return response switch
            {
                (Exception ex, null) => WriteError(ex),
                (null, HttpResponseMessage res) => await WriteResponse(res),
                _ => throw new InvalidOperationException()
            };
        }

        private static async Task<bool> WriteResponse(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStreamAsync();
            var parsedJson = await JsonSerializer.DeserializeAsync<dynamic>(responseBody);

            // using declarations - calls Dispose() when the variable goes out of scope
            using var stdout = Console.OpenStandardOutput();
            await JsonSerializer.SerializeAsync(
                stdout,
                parsedJson,
                new JsonSerializerOptions { WriteIndented = true });

            return true;
        }

        private static bool WriteError(Exception ex)
        {
            Console.WriteLine($"The request failed 😞 {ex.Message}");
            return false;
        }

        // And more besides!
        // - await foreach
        // - Default interface implementations (methods only)

        // Framework features not covered here:
        // - IAsyncDisposable
    }
}
