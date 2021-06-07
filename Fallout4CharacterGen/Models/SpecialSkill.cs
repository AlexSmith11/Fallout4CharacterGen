using System.Collections.Generic;

namespace Fallout4CharacterGen.Models
{
    public class SpecialSkill
    {
        public string SpecialName { get; set; }
        public int SpecialLevel { get; set; }
        public List<SpecialPerkRow> Rows { get; set; }
    }

    public class SpecialPerkRow
    {
        public string Name { get; set; }
        public string SpecialRankRequirement { get; set; }
        public string PerkRank { get; set; }
        public string PlayerLevelRequirement { get; set; }
        public string Description { get; set; }
        public string FormId { get; set; }
    }
}