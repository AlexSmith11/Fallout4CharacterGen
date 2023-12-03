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
        /// This must be run once to populate the database.
        ///
        /// No longer needed as the data has been processed and is now hosted on my GitHub.
        /// </summary>
        /// <returns></returns>
        public async Task LoadAndWriteData()
        {
            await _readerWriter.AskForFilename(); // Run the CSV & JSON reader writer program
        }
        
        /// <summary>
        /// Generate a character.
        /// Step 1)
        /// Download a list of perk information from the API.
        ///
        /// Step 2)
        /// Generate the list of a characters SPECIAL points.
        ///
        /// Step 3)
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task CreateCharacter()
        {
            // create a character
            var character = new Character();
            
            var allPerks = await _readerWriter.GetAllSpecialPerks();     //TODO: Refactor into the generation of the characters perks?
            var charactersSpecialRanks = await _specialManager.GenerateSpecialPoints();

            var characterWithPerks = await _specialManager.GeneratePerkLists(allPerks, charactersSpecialRanks);
            if (characterWithPerks.Contains(null)) return;

            Console.Write(characterWithPerks[3].SpecialName + ": ");
            Console.WriteLine(characterWithPerks[3].Perks.First().Name);
            Console.Write("The special level is: " + characterWithPerks[3].SpecialLevel);


            character.Special = characterWithPerks;

            /*
             * generate rest of character and parse to json
             *
             * list:
             * - Name
             * - weapon / build (energy: laser, melee: blunt, etc)  TODO: generate a JSON of all weapons
             * - companion
             * - settlements (where to consider 'home')
             * - disposition (unhinged, happy, sad, bad, etc)
             * - faction
             * - kleptomaniac (bool)
             */
            
            CharacterAssigner.getCompanion(character);

            
        }
    }
}
