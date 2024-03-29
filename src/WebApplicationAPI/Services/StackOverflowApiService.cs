using System.IO.Compression;
using System.Text.Json;
using Serilog;
using WebApplicationAPI.Interfaces.Repositories;
using WebApplicationAPI.Interfaces.Services;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Services
{
    public class StackOverflowApiService : IStackOverflowApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ITagRepository _tagRepository;

        public StackOverflowApiService(HttpClient httpClient, ITagRepository tagRepository)
        {
            _httpClient = httpClient;
            _tagRepository = tagRepository;
        }

        public async Task GetTagsAsync(int pagesToFetch)
        {
            _httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "deflate");

            int totalTagsCount = 0;
            var tags = new List<Tag>();

            for (int page = 1; page <= pagesToFetch; page++)
            {
                var response = await _httpClient.GetAsync(
                    $"https://api.stackexchange.com/2.3/tags?page={page}&pagesize=100&order=desc&sort=popular&site=stackoverflow");

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    var reader = new StreamReader(new DeflateStream(stream, CompressionMode.Decompress));
                    var jsonString = await reader.ReadToEndAsync();

                    var root = JsonDocument.Parse(jsonString).RootElement;
                    var items = root.GetProperty("items").EnumerateArray();

                    foreach (var item in items)
                    {
                        var tag = new Tag()
                        {
                            Name = item.GetProperty("name").GetString(),
                            Count = item.GetProperty("count").GetInt32()
                        };

                        totalTagsCount += tag.Count;

                        tags.Add(tag);
                    }
                }
                else
                {
                    Log.Error($"Error while fetching data | {response.RequestMessage.RequestUri}");
                }
            }

            tags.ForEach(tag => tag.Percentage = tag.Count * 100.0 / totalTagsCount);

            await _tagRepository.AddTagsAsync(tags);
            await _tagRepository.SaveChangesAsync();

            Log.Information("Tags added to database");
        }
    }
}
