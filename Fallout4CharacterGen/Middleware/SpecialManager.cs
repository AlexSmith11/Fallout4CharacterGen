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
        /// <para>
        /// Create a SPECIAL list and assign them points.
        /// </para>
        /// </summary>
        /// 
        /// <para>
        /// rules:
        /// - all special skills start at: 1 (STR: 1, PER: 1, etc), with maximum of 10
        /// - max number of points able to be spent at the start of the game: 28
        /// </para>
        ///
        /// <para>
        /// changed Random.Next() implementation due to thread issues. See here: https://csharpindepth.com/Articles/Random
        /// </para>
        /// 
        /// <code>
        /// method:
        /// if maxCounter > 10:
        /// assign random num between 1 - 10 to current special type
        /// take random num off maxCounter
        /// if maxCounter is smaller than 10:
        /// assign random num between 1 - maxCounter
        /// if points left over loop again - continue until maxCounter = 0
        /// </code>
        ///
        /// <para>
        /// to counter biasing (if it exists) the first special types listed, we select the special type to generate randomly
        /// i.e. select agility to assign to first, then perception, etc
        ///
        /// TODO: Make max counter an input from the user.
        /// 
        /// </para>
        /// <returns>list of special rankings</returns>
        public async Task<List<SpecialSkill>> GenerateSpecialPoints()
        {
            var maxCounter = 28;
            var specialTypesList = new List<SpecialSkill>();
            var tempSpecialNames = new List<SpecialInfo>(SpecialSkills.SpecialData());
            var rng = new Random();

            // Create a list of 7 Special classes (Str, Per, etc)
            // TODO: Why on earth are we doing this instead of just using SpecialSkill class? 3am things.
            for (int i = 6; i > -1; i--)
            {
                specialTypesList.Add(new SpecialSkill
                {
                    SpecialName = tempSpecialNames[i].SpecialName,
                    SpecialLevel = 1,
                    SpecialId = tempSpecialNames[i].SpecialId
                });
            }

            while (maxCounter > 0)
            {
                var specialNames = new List<SpecialInfo>(SpecialSkills.SpecialData());      // Get a new list to iterate over
                specialNames.Shuffle();

                for (var i = 6; i > -1; i--)
                {
                    // if all points are used up early, we quit while making sure all 7 are added to list
                    if (maxCounter <= 0) return specialTypesList;
                    
                    var tempCounter = maxCounter;                        // set the number of points we have to assign on this iteration
                    if (maxCounter >= 10) tempCounter = 10;                 // (either the max(10) or whatever we have left in maxCounter

                    var rndSkillLevel = MyExtensions.GenerateRngInt(rng, tempCounter);          // get rnd number of skill points to give to special name
                    if (rndSkillLevel <= 1) rndSkillLevel = 1;                                     // rng machine will always return 0 if tempCounter == 1
                    
                    var index = specialTypesList.FindIndex(x => x.SpecialName == specialNames[i].SpecialName);
                    specialTypesList[index].SpecialLevel += rndSkillLevel;
                    
                    specialNames.Remove(specialNames[i]);       // remove name of special type from list of types to create/add to so we don't use it again on this loop
                    maxCounter -= rndSkillLevel;                // deduct points used from total count left to spend
                }
            }

            return specialTypesList;
        }

        /// <summary>
        /// <para>
        /// Generate a list of perks for the player, and adds them to the players SPECIAL tree.
        /// </para>
        /// 
        /// <para>
        /// WARNING: this method is extremely brute-force-y and wasteful. there are far better ways to do this by
        /// restructuring the data models, such as having a tree structure ordered by special level and then perk req.
        /// </para>
        /// 
        /// <para>
        /// could just get a list of 40+ perks ordered by player lvl req. (PlayerLevelRequirement)
        /// need to make sure we get perk ranks in order - can't just have local leader 2 without rank 1. (PerkRank)
        /// must also make sure SpecialRankRequirement is met (req strength == 6 to get string back)
        /// data is given already in order
        /// </para>
        /// 
        /// <code>
        /// Rules:
        /// use characters special list (characterSpecialSkills) to contain all new perks - select ranges to display, don't need more than one list
        /// i = the player lvl
        /// add a perk that:
        /// - is not a duplicate
        /// - has a player req &lt; i
        /// - has special rank &lt; SpecialRankRequirement of perk
        /// - has to be either perk rank of 1 or rank of previous + 1 if there are already previous levels of same perk (1!)
        /// </code>
        /// 
        /// TODO: make sure that the minimum special level is 1, not 0
        /// TODO: refactor this so that it makes way more sense
        /// </summary>
        /// 
        /// <param name="allPerks">A list of SPECIAL with NO assigned values, plus EVERY perk that each SPECIAL can have.</param>
        /// <param name="characterSpecialSkills">List of SPECIAL with generated values for each, with NULL perk data</param>
        /// <returns></returns>
        public async Task<List<SpecialSkill>> GeneratePerkLists(List<SpecialSkill> allPerks, List<SpecialSkill> characterSpecialSkills)
        {
            if(allPerks is null || !allPerks.Any() || characterSpecialSkills is null || !characterSpecialSkills.Any()) return null;

            const int playerLevelCap = 40;  // TODO: make this changeable by user
            var rng = new Random();

            for (var playerLevel = 1; playerLevel < playerLevelCap + 1; playerLevel++)    // add one because of array index stuff
            {
                var randomSpecialIndex = 0;
                var randomPerkIndex = 0;
                
                // check perk - if doesn't work, get different perk
                var check = 0;
                while (check == 0)
                {
                    // select a random perk
                    var randomSpecialIndexTemp = MyExtensions.GenerateRngInt(rng, allPerks.Count);
                    var randomPerkIndexTemp = MyExtensions.GenerateRngInt(rng, allPerks[randomSpecialIndexTemp].Perks.Count);

                    var isPerkValid = true;
                    var tempSpecialToAddTo = allPerks[randomSpecialIndexTemp];
                    var tempPerkToAdd = allPerks[randomSpecialIndexTemp].Perks[randomPerkIndexTemp];
                    var characterSpecialSkill = characterSpecialSkills.FirstOrDefault(x => x.SpecialName.Equals(tempSpecialToAddTo.SpecialName));    // list of perks within the currently accessed special skill
                    if(characterSpecialSkill is null) continue;
                    characterSpecialSkill.Perks ??= new List<SpecialPerk>();    // if we do not have any perks yet, pass
                    
                    var characterPerks = characterSpecialSkill.Perks
                        .Where(x => x.Name.Equals(tempPerkToAdd.Name))
                        .OrderBy(x => x.PerkRank)
                        .ToList();    // perks we already have in the current special type
                    
                    // set default values just in case
                    if (tempPerkToAdd.PlayerLevelRequirement == "") tempPerkToAdd.PlayerLevelRequirement = "1";
                    if (tempPerkToAdd.SpecialRankRequirement == "") tempPerkToAdd.SpecialRankRequirement = "1";
                    
                    foreach (var characterPerk in characterPerks) 
                        if (characterPerk.FormId == tempPerkToAdd.FormId)
                            isPerkValid = false;
                    
                    if (!isPerkValid) continue;
                    
                    var temp = int.Parse(tempPerkToAdd.PlayerLevelRequirement);
                    if (temp > playerLevel) continue;
                    
                    if (int.Parse(tempPerkToAdd.SpecialRankRequirement) > characterSpecialSkill.SpecialLevel) continue;

                    var tempPerkToAddRank = int.Parse(tempPerkToAdd.PerkRank);
                    if (tempPerkToAddRank != 1)
                    {
                        var currentPerkLevel = characterPerks.Count;
                        if(currentPerkLevel + 1 != tempPerkToAddRank) continue;
                    }
                    
                    // set special skill id
                    characterSpecialSkill.SpecialId = tempSpecialToAddTo.SpecialId;

                    // persist the perks selected
                    randomSpecialIndex = randomSpecialIndexTemp;
                    randomPerkIndex = randomPerkIndexTemp;
                    check = 1;  // can't add the perk until all previous checks have been made
                }

                var randomSpecialType = allPerks[randomSpecialIndex];
                var randomPerk = allPerks[randomSpecialIndex].Perks[randomPerkIndex];
                
                // after checking perk, add it to appropriate special type in character
                characterSpecialSkills.First(x => x.SpecialName.Equals(randomSpecialType.SpecialName)).Perks.Add(
                    new SpecialPerk
                    {
                        Name = randomPerk.Name,
                        SpecialRankRequirement = randomPerk.SpecialRankRequirement,
                        PerkRank = randomPerk.PerkRank,
                        PlayerLevelRequirement = randomPerk.PlayerLevelRequirement,
                        Description = randomPerk.Description,
                        FormId = randomPerk.FormId
                    }
                );
            }

            return characterSpecialSkills.OrderBy(x => x.SpecialId).ToList();
        }
    }
}