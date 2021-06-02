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
            var data = CsvReader.LoadCsvData();
            
            JsonParser.WriteToDisk(data, "strength");
        }
    }
}