using System;
using System.Threading.Tasks;

namespace Fallout4CharacterGen
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            // store API calls here
            await ApiCalls();
            
            LoadData();
        }

        private static async Task ApiCalls()
        {
            
        }

        private static void LoadData()
        {
            var specialType = "agility";
            
            var data = CsvReader.LoadCsvData(specialType);
            JsonParser.WriteToDisk(data, specialType);
            var deserializedSpecialPerkData = JsonParser.ReadSpecialSkillFromDisk(specialType);
            
        }
    }
}