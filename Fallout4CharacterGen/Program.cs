using System;
using System.Threading.Tasks;
using Fallout4CharacterGen;
using Fallout4CharacterGen.DatabaseSource.Local;
using Fallout4CharacterGen.Interfaces;
using Fallout4CharacterGen.Middleware;

namespace Fallout4CharacterGen
{
    public class Program
    {
        private readonly ICsvJsonReaderWriter _readerWriter = new CsvAndJsonReaderWriter();
        private readonly ICharacterBuilder _characterBuilder = new CharacterBuilder();

        private static async Task Main(string[] args)
        {
            var userInterface = new Program();
            await userInterface.CreateCharacter();
        }
        
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
        /// Generate a character and export it as a JSON file.
        /// </summary>
        /// <returns></returns>
        private async Task CreateCharacter()
        {
            /*
             * generate character and parse to json
             *
             * list:
             * - Name                                          [DONE]
             * - companion                                     [DONE]
             * - settlements (where to consider 'home')        [DONE]
             * - perks                                         [DONE]
             * - disposition (unhinged, happy, sad, bad, etc)
             * * - weapon / build (energy: laser, melee: blunt, etc)  TODO: generate a JSON of all weapons
             * - faction
             * - kleptomaniac (bool)
             */

            var character = await _characterBuilder.BuildCharacter();

            // Export character Data for frontend:
            JsonParser.WriteCharacterToDisk(character, character.Name);
        }
    }
}