using System;
using System.IO;
using System.Threading.Tasks;
using Fallout4CharacterGen.Interfaces;
using Fallout4CharacterGen.Middleware;

namespace Fallout4CharacterGen
{
    class Program
    {
        private static readonly ICsvJsonReaderWriter ReaderWriter = new CsvAndJsonReaderWriter();
        
        private static async Task Main(string[] args)
        {
            // await ReaderWriter.AskForFilename(); // Run the CSV & JSON reader writer program

            await GetStuff();
        }

        private static async Task GetStuff()
        {
            var temp = await ReaderWriter.GetSpecialSkillData();

            var temp2 = 1^2;
        }
    }
}