using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fallout4CharacterGen.Interfaces;
using Fallout4CharacterGen.Middleware;
using Fallout4CharacterGen.Models;
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
        
        /// <summary>
        /// generate a character
        /// </summary>
        /// <returns></returns>
        public async Task CreateCharacter()
        {
            /*
             * generate a characters special list
             */
            var perkList = await _readerWriter.GetAllSpecialPerks();
            var charactersSpecialRanks = await _specialManager.GenerateSpecialPoints();

            var character = await _specialManager.GeneratePerkLists(perkList, charactersSpecialRanks);
            if (character.Contains(null)) return; // does this actually work ????? lmao

            Console.Write(character[3].SpecialName + ": ");
            Console.Write(character[3].Perks.First().Name);
            
            /*
             * generate rest of character
             */
        }
    }
}
