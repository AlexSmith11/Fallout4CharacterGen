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
            var specialList = await ReaderWriter.GetSpecialSkillData();
            
            // get random perk
            var randomNum = new Random();
            var randomIndex1 = randomNum.Next(specialList.Count);
            var randomIndex2 = randomNum.Next(specialList[randomIndex1].Rows.Count);
            
            Console.Write(specialList[randomIndex1].SpecialName + ": ");
            Console.WriteLine(specialList[randomIndex1].Rows[randomIndex2].Name);
        }
    }
}