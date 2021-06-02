using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen
{
    public class JsonParser
    {
        public static void WriteToDisk(List<CsvRow> csvRows, string specialType)
        {
            var path = @"C:\Users\alexs\Desktop\fallout4_csv\special\";
            var fileName = $"{specialType}.json";
            
            var json = JsonSerializer.Serialize<List<CsvRow>>(csvRows);
            File.WriteAllText(path + fileName, json);
        }

        public static List<CsvRow> ReadSpecialSkillFromDisk(string specialName)
        {
            
            
            return null;
        }
    }
}