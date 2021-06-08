using System.Collections.Generic;

namespace Fallout4CharacterGen.Models
{
    public class SpecialSkill
    {
        public SpecialSkill()
        {
            this.SpecialLevel = 1;
        }
        public int SpecialId { get; set; }
        public string SpecialName { get; set; }
        public int SpecialLevel { get; set; } = 1;
        public List<SpecialPerk> Perks { get; set; }
    }

    public class SpecialPerk
    {
        public string Name { get; set; }
        public string SpecialRankRequirement { get; set; } = "1";
        public string PerkRank { get; set; }
        public string PlayerLevelRequirement { get; set; } = "1";
        public string Description { get; set; }
        public string FormId { get; set; }
        public string ParentSpecialName { get; set; }
    }
}