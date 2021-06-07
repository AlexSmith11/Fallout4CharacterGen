using System.Collections.Generic;

namespace Fallout4CharacterGen.DatabaseSource.Local
{
    public static class SpecialSkills
    {
        public static List<string> SpecialNames { get; set; } = new()
            {"strength", "perception", "endurance", "charisma", "intelligence", "agility", "luck"};
    }
}