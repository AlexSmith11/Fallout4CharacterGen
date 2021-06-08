using System.Collections.Generic;
using System.Threading.Tasks;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen.Interfaces
{
    public interface ISpecialManager
    {
        Task<List<SpecialSkill>> GenerateSpecialPoints();

        Task<List<SpecialSkill>> GeneratePerkLists(List<SpecialSkill> perkList, List<SpecialSkill> characterSpecialSkills);
    }
}