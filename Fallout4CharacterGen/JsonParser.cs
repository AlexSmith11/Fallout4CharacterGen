using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen
{
    public static class JsonParser
    {
        const string path = @"C:\Users\alexs\Desktop\fallout4_csv\special\json\";
        
        public static void WriteToDisk(List<CsvRow> csvRows, string specialType)
        {
            var fileName = $"{specialType}.json";
            
            var json = JsonSerializer.Serialize<List<CsvRow>>(csvRows);
            File.WriteAllText(path + fileName, json);
        }

        public static List<CsvRow> ReadSpecialSkillFromDisk(string specialName)
        {
            using (var streamReader = new StreamReader(path + specialName + ".json"))
            {
                var json = streamReader.ReadToEnd();
                var specialPerks = JsonSerializer.Deserialize<List<CsvRow>>(json);

                return specialPerks;
            }
        }
    }
}