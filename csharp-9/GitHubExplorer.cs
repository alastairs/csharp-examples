using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace csharp_9
{
    public class GitHubExplorer
    {
        private static readonly HttpClient HttpClient;

        private static readonly JsonSerializerOptions JsonSerializerConfiguration;

        static GitHubExplorer()
        {
            HttpClient = new HttpClient(new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(5)
            });

            JsonSerializerConfiguration = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
                WriteIndented = true
            };
        }

        public async Task ExploreAsync(string uri = "https://api.github.com/")
        {
            HttpClient.BaseAddress = new Uri(uri);
            HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd($"Mozilla/5.0 (compatible; github.com/alastairs/csharp-examples)");

            var jsonStream = (await HttpClient.GetByteArrayAsync("/")).AsMemory();
            var response = JsonSerializer.Deserialize<Responses.IndexResponse>(jsonStream.Span, JsonSerializerConfiguration);

            Console.WriteLine($"Repository URL is: '{response?.RepositoryUrl ?? "Deserialisation failed"}'");
        }
    }
}