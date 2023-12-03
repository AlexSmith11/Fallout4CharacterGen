using System.Collections.Generic;

namespace Fallout4CharacterGen.Models
{
    /// <summary>
    /// An individual character, with assigned SPECIAL skills, their companion, home, etc.
    /// </summary>
    public class Character
    {
        public List<SpecialSkill> Special { get; set; }
        
        public string Companion { get; set; }
        
        public string HomeSettlement { get; set; }
    }
}