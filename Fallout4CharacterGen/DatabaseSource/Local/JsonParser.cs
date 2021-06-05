using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen.Source.Local
{
    public static class JsonParser
    {
        const string folderPath = @"C:\Users\alexs\Desktop\fallout4_csv\special\json\";
        
        /// <summary>
        /// JSON Writer
        /// </summary>
        /// <param name="csvRows"></param>
        /// <param name="specialType"></param>
        public static void WriteToDisk(List<SpecialPerkCsvRow> csvRows, string specialType)
        {
            var fileName = folderPath + specialType + ".json";
            
            var json = JsonSerializer.Serialize<List<SpecialPerkCsvRow>>(csvRows);
            File.WriteAllText(fileName, json);
        }

        /// <summary>
        /// JSON reader
        /// </summary>
        /// <param name="specialName"></param>
        /// <returns></returns>
        public static List<SpecialPerkCsvRow> ReadSpecialSkillFromDisk(string specialName)
        {
            var filePath = folderPath + specialName + ".json";
            
            using (var streamReader = new StreamReader(filePath))
            {
                var json = streamReader.ReadToEnd();
                var specialPerks = JsonSerializer.Deserialize<List<SpecialPerkCsvRow>>(json);

                return specialPerks;
            }
        }
    }
}