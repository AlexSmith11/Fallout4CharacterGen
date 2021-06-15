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
            var tempSpecialNames = new List<SpecialInfo>(SpecialSkills.SpecialData());
            var rng = new Random();

            // populate list. this ofc makes a ton of the below redundant but i'm done for now
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
                specialNames.Shuffle();                                               // shuffle/randomise list

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
        /// generate a list of perks for the player, ordered by player level
        ///
        /// WARNING: this method is extremely brute-forcey & wasteful. there are far better ways to do this by
        /// restructuring the data models, such as having a tree structure ordered by special level and then perk req.
        ///
        /// TODO: make sure that the minimum special level is 1, not 0
        /// TODO: refactor this so that it makes way more sense
        /// </summary>
        /// <returns></returns>
        public async Task<List<SpecialSkill>> GeneratePerkLists(List<SpecialSkill> perkList, List<SpecialSkill> characterSpecialSkills)
        {
            // could just get a list of 40+ perks ordered by player lvl req. (PlayerLevelRequirement)
            // need to make sure we get perk ranks in order - can't just have local leader 2 without rank 1. (PerkRank)
            // must also make sure SpecialRankRequirement is met (req strength == 6 to get string back)
            // data is given already in order
            
            // use characters special list to contain all new perks - select ranges to display, don't need more than one list
            // for loop? where i = the player lvl
            // add a perk that:
            // - is not a duplicate
            // - has a player req < i
            // - has special rank < SpecialRankRequirement of perk
            // - has to be either perk rank of 1 or rank of previous + 1 if there are already previous levels of same perk (1!)

            // (1!): just skip the perk.
            
            if(perkList is null || !perkList.Any() || characterSpecialSkills is null || !characterSpecialSkills.Any()) return null;

            const int playerLevelCap = 40;  // make this changeable by user
            var rng = new Random();

            for (var i = 1; i < playerLevelCap + 1; i++)    // add one because of array index stuff 
            {
                var randomSpecialIndex = 0;
                var randomPerkIndex = 0;
                
                // check perk - if doesn't work, get different perk
                var check = 0;
                while (check == 0)
                {
                    // select a random perk
                    var randomSpecialIndexTemp = MyExtensions.GenerateRngInt(rng, perkList.Count);
                    var randomPerkIndexTemp = MyExtensions.GenerateRngInt(rng, perkList[randomSpecialIndexTemp].Perks.Count);

                    var isPerkValid = true;
                    var tempSpecialToAddTo = perkList[randomSpecialIndexTemp];
                    var tempPerkToAdd = perkList[randomSpecialIndexTemp].Perks[randomPerkIndexTemp];
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
                    if (temp > i) continue;
                    
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

                var randomSpecialType = perkList[randomSpecialIndex];
                var randomPerk = perkList[randomSpecialIndex].Perks[randomPerkIndex];
                
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