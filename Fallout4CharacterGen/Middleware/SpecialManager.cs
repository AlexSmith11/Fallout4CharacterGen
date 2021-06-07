using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fallout4CharacterGen.DatabaseSource.Local;
using Fallout4CharacterGen.Interfaces;
using Fallout4CharacterGen.Models;
using Fallout4CharacterGen.Utilities;

namespace Fallout4CharacterGen.Middleware
{
    public class SpecialManager : ISpecialManager
    {
        /// <summary>
        /// rules:
        /// - all special skills start at: 1
        /// - max number of points spent: 28
        /// - max any special skill can be: 10
        ///
        /// changed Random.Next() implementation due to thread issues. See here: https://csharpindepth.com/Articles/Random
        ///
        /// method:
        /// if maxCounter > 10:
        /// assign random num between 1 - 10 to current special type
        /// take random num off maxCounter
        /// if maxCounter is smaller than 10:
        /// assign random num between 1 - maxCounter
        /// if points left over loop again - continue until maxCounter = 0
        /// 
        /// to counter biasing (if it exists) the first special types listed, we select the special type to generate randomly
        /// i.e. select agility to assign to first, then perception, etc
        /// 
        /// </summary>
        /// <returns>list of special rankings</returns>
        public async Task<List<SpecialSkill>> GenerateSpecialPoints()
        {
            var maxCounter = 28;
            var specialTypesList = new List<SpecialSkill>();
            var rng = new Random();

            while (maxCounter > 0)
            {
                var specialNames = new List<string>(SpecialSkills.SpecialNames);      // Get a new list to iterate over   TODO: this doesn't refresh after one loop
                specialNames.Shuffle();                                               // shuffle/randomise list

                for (var i = 6; i > -1; i--)
                {
                    if (maxCounter <= 0) return specialTypesList;
                    var tempCounter = maxCounter;                       // set the number of points we have to assign on this iteration
                    if (maxCounter >= 10) tempCounter = 10;                 // (either the max(10) or whatever we have left in maxCounter

                    var rndSkillLevel = MyExtensions.GenerateRngInt(rng, tempCounter);       // get rnd number of skill points to give to special name
                    if (maxCounter == 1) rndSkillLevel = 1;                                     // rng machine will always return 0 if tempCounter == 1

                    // if the skill already exists, add to that instead of making a new one
                    if (specialTypesList.Any(x => x.SpecialName == specialNames[i]))
                    {
                        var index = specialTypesList.FindIndex(x => x.SpecialName == specialNames[i]);
                        specialTypesList[index].SpecialLevel += rndSkillLevel;
                    }
                    else
                    {
                        specialTypesList.Add(new SpecialSkill
                            {SpecialName = specialNames[i], SpecialLevel = rndSkillLevel});
                    }

                    specialNames.Remove(specialNames[i]);       // remove name of special type from list of types to create/add to so we don't use it again on this loop
                    maxCounter -= rndSkillLevel;                // deduct points used from total count left to spend
                }
            }

            return specialTypesList;
        }
    }
}