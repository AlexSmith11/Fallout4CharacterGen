using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fallout4CharacterGen.DatabaseSource.Local;
using Fallout4CharacterGen.Interfaces;
using Fallout4CharacterGen.Middleware;
using Fallout4CharacterGen.Models;
using Fallout4CharacterGen.Utilities;

namespace Fallout4CharacterGen
{
    class Program
    {
        private static readonly ICsvJsonReaderWriter ReaderWriter = new CsvAndJsonReaderWriter();

        private static async Task Main(string[] args)
        {
            // await ReaderWriter.AskForFilename(); // Run the CSV & JSON reader writer program

            await DoStuff();
        }

        private static async Task DoStuff()
        {
            var specialList = await ReaderWriter.GetSpecialSkillData();

            // get random perk
            var randomNum = new Random();
            var randomIndexSpecial = randomNum.Next(specialList.Count);
            var randomIndexPerk = randomNum.Next(specialList[randomIndexSpecial].Rows.Count);

            Console.Write(specialList[randomIndexSpecial].SpecialName + ": ");
            Console.WriteLine(specialList[randomIndexSpecial].Rows[randomIndexPerk].Name);

            var temp = GenerateSpecialPoints();
            var gbfd = 1 + 4;
        }

        /// <summary>
        /// rules:
        /// - all special skills start at: 1
        /// - max number of points spent: 28
        /// - max any special skill can be: 10
        /// </summary>
        /// <returns></returns>
        private static async Task<List<SpecialSkill>> GenerateSpecialPoints()
        {
            var maxCounter = 28;
            var specialTypesList = new List<SpecialSkill>();

            // method 1:
            // if maxCounter > 10:
            // assign random num between 1 - 10 to current special type
            // take random num off maxCounter
            // if maxCounter < 10:
            // assign random num between 1 - maxCounter
            // if points left over loop again - continue until maxCounter = 0

            // to counter biasing (if it exists) the first special types listed, we select the special type to generate randomly
            // i.e. select agility to assign to first, then perception, etc

            while (maxCounter > 0)
            {
                // var usedSpecialNames = SpecialSkills.SpecialNames;  // need to keep track of which types we have already iterated over. once we have used all once, reset this list and go until all points are spent 
                var specialNames = new List<string>(SpecialSkills.SpecialNames);      // Get a new list to iterate over   TODO: this doesn't refresh after one loop
                specialNames.Shuffle();                                               // shuffle/randomise list

                for (var i = 6; i > -1; i--)
                {
                    if (maxCounter <= 0) return specialTypesList;
                    var tempCounter = maxCounter;           // set the number of points we have to assign on this iteration
                    if (maxCounter >= 10) tempCounter = 10;    // (either the max(10) or whatever we have left in maxCounter

                    var rnd = new Random();
                    var rndSkillLevel = rnd.Next(tempCounter);          // get rnd number of skill points to give to special name

                    // if the skill already exists, add to that instead
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