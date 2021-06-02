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
            var fileName = path + specialType + ".json";
            
            var json = JsonSerializer.Serialize<List<CsvRow>>(csvRows);
            File.WriteAllText(fileName, json);
        }

        public static List<CsvRow> ReadSpecialSkillFromDisk(string specialName)
        {
            var filePath = path + specialName + ".json";
            
            using (var streamReader = new StreamReader(filePath))
            {
                var json = streamReader.ReadToEnd();
                var specialPerks = JsonSerializer.Deserialize<List<CsvRow>>(json);

                return specialPerks;
            }
        }
    }
}