using System;
using System.Threading.Tasks;
using Fallout4CharacterGen.Interfaces;
using Fallout4CharacterGen.Middleware;
using Fallout4CharacterGen.Utilities;

namespace Fallout4CharacterGen
{
    public class DoStuff
    {
        private readonly ICsvJsonReaderWriter _readerWriter = new CsvAndJsonReaderWriter();
        private readonly ISpecialManager _specialManager = new SpecialManager();

        public async Task LoadAndWriteData()
        {
            await _readerWriter.AskForFilename(); // Run the CSV & JSON reader writer program
        }
        
        public async Task DoThings()
        {
            /*
             * generate a special list
             */
            var perkList = await _readerWriter.GetAllSpecialPerks();

            // get random perk
            var rng = new Random();
            var randomIndexSpecial = MyExtensions.GenerateRngInt(rng, perkList.Count);
            var randomIndexPerk = MyExtensions.GenerateRngInt(rng, perkList[randomIndexSpecial].Rows.Count);
            
            Console.Write(perkList[randomIndexSpecial].SpecialName + ": ");
            Console.WriteLine(perkList[randomIndexSpecial].Rows[randomIndexPerk].Name);

            
            var charactersSpecial = await _specialManager.GenerateSpecialPoints();
            
            // now need to match the two lists and merge them
            var gbfd = 1 + 4;
        }
    }
}