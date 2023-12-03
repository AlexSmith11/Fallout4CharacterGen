using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen.DatabaseSource.Remote
{
    /// <summary>
    /// GET request for the SPECIAL data hosted on github.
    /// SPECIAL data: there are 7 subclasses of special perks:
    /// Strength, Perception, Endurance, Charisma, Intelligence, Agility, Luck.
    ///
    /// Each Subclass has a list of perks that can be gained by the character, with each perk having an assigned tier.
    /// The character must reach a perks assigned tier in order to unlock and activate it.
    ///
    /// @TODO: Maybe inject Client rather than create it here. No other use of it is so far required however. 
    /// </summary>
    public class GetApiData
    {
        private static readonly HttpClient Client = new();
        private const string JsonUrl = "https://alexsmith11.github.io/fallout4_data/special/json/";

        /// <summary>
        /// Get a list of a user given subclass of SPECIAL perks, e.g. "luck".
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
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