using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen.DatabaseSource.Remote
{
    public class GetApiData
    {
        private static readonly HttpClient Client = new();
        private const string JsonUrl = "https://alexsmith11.github.io/fallout4_data/special/json/";

        public static async Task<List<SpecialPerk>> GetSpecialJson(string userInput)
        {
            var url = $"{JsonUrl}{userInput}.json";

            var response = await Client.GetAsync(url);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            
            var specialPerks = JsonSerializer.Deserialize<List<SpecialPerk>>(responseContent);

            return specialPerks;
        }
    }
}