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

        /// <summary>
        /// This must be run once to populate the database
        /// </summary>
        /// <returns></returns>
        public async Task LoadAndWriteData()
        {
            await _readerWriter.AskForFilename(); // Run the CSV & JSON reader writer program
        }
        
        public async Task CreateCharacter()
        {
            /*
             * generate a characters special list
             */
            var perkList = await _readerWriter.GetAllSpecialPerks();

            // get random perk
            var rng = new Random();
            var randomIndexSpecial = MyExtensions.GenerateRngInt(rng, perkList.Count);
            var randomIndexPerk = MyExtensions.GenerateRngInt(rng, perkList[randomIndexSpecial].Rows.Count);
            
            Console.Write(perkList[randomIndexSpecial].SpecialName + ": ");
            Console.WriteLine(perkList[randomIndexSpecial].Rows[randomIndexPerk].Name);

            
            var charactersSpecialRanks = await _specialManager.GenerateSpecialPoints();
            
            // now need to match the two lists and merge them
        }

        public async Task GeneratePerkLists()
        {
            // need perk lists of:
            // player lvl 1 - 5
            // player lvl 6 - 10
            // player lvl 10 - 20
            // player lvl 20 - 30
            // player lvl 30 - 40
            // player lvl 40+
            
            // new List<SpecialPerkRow> for each of these ranges
            
            // for loop? where i = the player lvl
        }
    }
}