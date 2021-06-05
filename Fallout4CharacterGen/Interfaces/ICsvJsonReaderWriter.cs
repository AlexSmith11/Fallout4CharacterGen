using System.Collections.Generic;
using System.Threading.Tasks;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen.Interfaces
{
    public interface ICsvJsonReaderWriter
    {
        public Task<List<SpecialSkill>> GetSpecialSkillData();
    }
}