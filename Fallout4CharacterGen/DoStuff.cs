using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fallout4CharacterGen.Interfaces;
using Fallout4CharacterGen.Middleware;
using Fallout4CharacterGen.Models;
using Fallout4CharacterGen.Utilities;

namespace Fallout4CharacterGen
{
    public class DoStuff
    {
        private readonly ICsvJsonReaderWriter _readerWriter = new CsvAndJsonReaderWriter();
        private readonly ISpecialManager _specialManager = new SpecialManager();

        /// <summary>
        /// This must be run once to populate the database
        /// </summary>
        /// <returns></returns>
        public async Task LoadAndWriteData()
        {
            await _readerWriter.AskForFilename(); // Run the CSV & JSON reader writer program
        }
        
        public async Task CreateCharacter()
        {
            /*
             * generate a characters special list
             */
            var perkList = await _readerWriter.GetAllSpecialPerks();
            var charactersSpecialRanks = await _specialManager.GenerateSpecialPoints();

            // get random perk
            var rng = new Random();
            var randomIndexSpecial = MyExtensions.GenerateRngInt(rng, perkList.Count);
            var randomIndexPerk = MyExtensions.GenerateRngInt(rng, perkList[randomIndexSpecial].Perks.Count);
            
            Console.Write(perkList[randomIndexSpecial].SpecialName + ": ");
            Console.WriteLine(perkList[randomIndexSpecial].Perks[randomIndexPerk].Name);
            
            
            // now need to match the two lists and merge them(?)
            await GeneratePerkLists(perkList, charactersSpecialRanks);

            var temp = 1 ^ 2;
        }

        // refactor into SpecialManager
        /// <summary>
        /// generate a list of perks for the player, ordered by player level
        /// </summary>
        /// <returns></returns>
        public async Task GeneratePerkLists(List<SpecialSkill> perkList, List<SpecialSkill> characterSpecialSkills)
        {
            const int playerLevelCap = 80;  // make this changeable by user
            var rng = new Random();

            // need perk lists of:
            // player lvl 1 - 5
            // player lvl 6 - 10
            // player lvl 10 - 20
            // player lvl 20 - 30
            // player lvl 30 - 40
            // player lvl 40+

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

            for (var i = 0; i < playerLevelCap; i++)
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
                    var characterSpecialSkill = characterSpecialSkills.First(x => x.SpecialName.Equals(tempSpecialToAddTo.SpecialName));    // list of perks within the currently accessed special skill
                    characterSpecialSkill.Perks ??= new List<SpecialPerk>();    // make sure the perk list is not null
                    
                    var characterPerks = characterSpecialSkill.Perks
                        .Where(x => x.Name.Equals(tempPerkToAdd.Name)).OrderBy(x => x.PerkRank).ToList();    // perks we already have in the current special type
                    
                    // set default values
                    if (tempPerkToAdd.PlayerLevelRequirement == "") tempPerkToAdd.PlayerLevelRequirement = "1";
                    if (tempPerkToAdd.SpecialRankRequirement == "") tempPerkToAdd.SpecialRankRequirement = "1";
                    
                    foreach (var characterPerk in characterPerks)
                    {
                        if (characterPerk.FormId == tempPerkToAdd.FormId) isPerkValid = false;
                    }
                    if (!isPerkValid) continue;
                    
                    var temp = int.Parse(tempPerkToAdd.PlayerLevelRequirement);
                    if (temp > i)
                    {
                        continue;
                    }
                    
                    if (int.Parse(tempPerkToAdd.SpecialRankRequirement) > characterSpecialSkill.SpecialLevel)
                    {
                        continue;
                    }

                    var tempPerkToAddRank = int.Parse(tempPerkToAdd.PerkRank);
                    if (tempPerkToAddRank != 1)
                    {
                        var currentPerkLevel = characterPerks.Count;
                        if(currentPerkLevel + 1 != tempPerkToAddRank) continue;
                    }
                    
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
        }
    }
}
































