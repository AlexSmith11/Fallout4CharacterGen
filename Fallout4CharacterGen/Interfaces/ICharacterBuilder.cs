using System.Threading.Tasks;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen.Interfaces
{
    public interface ICharacterBuilder
    {
        public Task<Character> BuildCharacter();
    }
}