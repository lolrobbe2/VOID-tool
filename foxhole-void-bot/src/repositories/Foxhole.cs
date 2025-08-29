using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FoxholeBot.repositories 
{
    public class FoxholeRepository
    {
        HttpClient client;
        string[] regions;
        public FoxholeRepository()
        {
            client = new HttpClient();
            regions = GetRegions();
        }

        public string[] GetAllRegions()
        {
            return regions
                  .Select(region =>
                      Regex.Replace(region.Replace("Hex", ""), "(?<!^)([A-Z])", " $1")
                  )
                  .ToArray();
        }

        private string[] GetRegions()
        {

            HttpResponseMessage response = client.GetAsync($"{Config.GetFoxholeShardURL()}/worldconquest/maps").GetAwaiter().GetResult();
            string json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            // Deserialize into string array
            return JsonSerializer.Deserialize<string[]>(json);
        }
    }
}