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
            var specialList = await _readerWriter.GetSpecialSkillData();

            // get random perk
            var rng = new Random();
            var randomIndexSpecial = MyExtensions.GenerateRngInt(rng, specialList.Count);
            var randomIndexPerk = MyExtensions.GenerateRngInt(rng, specialList[randomIndexSpecial].Rows.Count);
            
            Console.Write(specialList[randomIndexSpecial].SpecialName + ": ");
            Console.WriteLine(specialList[randomIndexSpecial].Rows[randomIndexPerk].Name);

            
            var tempSpecialListRanks = await _specialManager.GenerateSpecialPoints();
            
            // now need to match the two lists and merge them
            var gbfd = 1 + 4;
        }
    }
}