using System.Collections.Generic;
using System.Threading.Tasks;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen.Interfaces
{
    public interface ICsvJsonReaderWriter
    {
        /// <summary>
        /// Return a list of pre-generated special perks
        /// </summary>
        /// <returns></returns>
        public Task<List<SpecialSkill>> GetAllSpecialPerks();

        /// <summary>
        /// Return the users chosen character name
        /// </summary>
        /// <returns></returns>
        public Task<string> AskForCharacterName();
        
        /// <summary>
        /// generate special perks from CSVs
        /// </summary>
        /// <returns></returns>
        public Task AskForFilename();
    }
}