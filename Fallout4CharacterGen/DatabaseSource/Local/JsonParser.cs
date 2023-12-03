using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen.DatabaseSource.Local
{
    public static class JsonParser
    {
        private const string FolderPath = @"C:\Users\alexs\Desktop\fallout4_csv\special\json\";
        private const string CharacterExportPath = @"C:\Users\alexs\Desktop\f4export\";
        
        /// <summary>
        /// JSON Writer that creates a JSON file of a list of a SPECIAL classes perks.
        /// </summary>
        /// <param name="csvRows"></param>
        /// <param name="specialType"></param>
        public static void WriteSpecialPerkListToDisk(List<SpecialPerk> csvRows, string specialType)
        {
            var fileName = FolderPath + specialType + ".json";
            
            var json = JsonSerializer.Serialize<List<SpecialPerk>>(csvRows);
            File.WriteAllText(fileName, json);
        }
        
        /// <summary>
        /// TODO: remove spaces from char name filepath.
        /// TODO: Merge this and above method and give file path as param.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="characterName"></param>
        public static void WriteCharacterToDisk(Character character, string characterName)
        {
            var fileName = CharacterExportPath + characterName + ".json";
            
            var json = JsonSerializer.Serialize<Character>(character);
            File.WriteAllText(fileName, json);
        }

        /// <summary>
        /// JSON reader
        /// </summary>
        /// <param name="specialName"></param>
        /// <returns></returns>
        public static List<SpecialPerk> ReadSpecialSkillFromDisk(string specialName)
        {
            var filePath = FolderPath + specialName + ".json";
            
            using (var streamReader = new StreamReader(filePath))
            {
                var json = streamReader.ReadToEnd();
                var specialPerks = JsonSerializer.Deserialize<List<SpecialPerk>>(json);

                return specialPerks;
            }
        }
        
        // TODO: Parse character obj to JSON to send to front end
    }
}