using System.Collections.Generic;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen.DatabaseSource.Local
{
    public static class SpecialSkills
    {
        /// <summary>
        /// Create a list of <value>SpecialInfo</value>'s & populates then with their Names and Id.
        /// </summary>
        /// <returns></returns>
        public static List<SpecialInfo> SpecialData()
        {
            var newSpecialData = SpecialNames;
            var specialData = new List<SpecialInfo>();

            for (var i = 0; i < newSpecialData.Count; i++)
            {
                var specialType = new SpecialInfo()
                {
                    SpecialId = i,
                    SpecialName = newSpecialData[i]
                };

                specialData.Add(specialType);
            }

            return specialData;
        }

        public static List<string> SpecialNames { get; } = new()
            {"strength", "perception", "endurance", "charisma", "intelligence", "agility", "luck"};
    }
}