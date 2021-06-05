using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen.DatabaseSource.Local
{
    public static class JsonParser
    {
        private const string FolderPath = @"C:\Users\alexs\Desktop\fallout4_csv\special\json\";
        
        /// <summary>
        /// JSON Writer
        /// </summary>
        /// <param name="csvRows"></param>
        /// <param name="specialType"></param>
        public static void WriteToDisk(List<SpecialPerkRow> csvRows, string specialType)
        {
            var fileName = FolderPath + specialType + ".json";
            
            var json = JsonSerializer.Serialize<List<SpecialPerkRow>>(csvRows);
            File.WriteAllText(fileName, json);
        }

        /// <summary>
        /// JSON reader
        /// </summary>
        /// <param name="specialName"></param>
        /// <returns></returns>
        public static List<SpecialPerkRow> ReadSpecialSkillFromDisk(string specialName)
        {
            var filePath = FolderPath + specialName + ".json";
            
            using (var streamReader = new StreamReader(filePath))
            {
                var json = streamReader.ReadToEnd();
                var specialPerks = JsonSerializer.Deserialize<List<SpecialPerkRow>>(json);

                return specialPerks;
            }
        }
    }
}