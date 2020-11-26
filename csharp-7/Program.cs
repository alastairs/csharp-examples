using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace csharp_7
{
    public class Program
    {
        // Expression-bodied constructor. Get and set accessors, and finalizers are also supported now.
        // `in` parameter cannot be updated within the method - similar to `readonly` fields.
        public Program(in Encoding consoleEncoding = null) => Console.OutputEncoding = consoleEncoding ?? Encoding.UTF8;

        // async Main
        public static async Task Main(string[] args)
        {
            await new Program().RunAsync();
        }

        public async Task RunAsync()
        {
            // Local function, returning a ValueTuple
            async Task<(Exception, HttpResponseMessage)> SendRequest(string url)
            {
                // throw expression
                url = url ?? throw new ArgumentNullException(nameof(url));

                Console.WriteLine("Calling GitHub 🐙🐱");
                try
                {
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.UserAgent.ParseAdd($"Mozilla/5.0 (compatible; github.com/alastairs/csharp-examples)");
                    var response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();
                    return (null, response);
                }
                catch (Exception ex) when (ex is HttpRequestException) // C# 6 Exception filtering
                {
                    return (ex, null);
                }
            }

            // Tuple destructuring, and supports discarding any tuple member with _
            var (exception, httpResponse) = await SendRequest("https://api.github.com/");

            // Pattern matching the exception, for an inelegant Either<Error, Result> implementation
            switch ((exception, httpResponse))
            {
                case var _ when exception is HttpRequestException:
                    Console.WriteLine($"The request failed 😞 {exception.Message}");
                    return;

                case var _ when exception is null:
                    var responseBody = await httpResponse.Content.ReadAsStringAsync();
                    dynamic parsedJson = JsonConvert.DeserializeObject(responseBody);

                    await Console.WriteLine(JsonConvert.SerializeObject(parsedJson, Formatting.Indented));
                    return;
            };
        }

        // And more besides!
        // * `default` literal expressions
        // * _ can be used as a digit separator in numeric constants
        // * `out` variables can now be declared inline, with an inferred type (var) if desired
        // * New `private protected` access modifier for class members
    }
}
