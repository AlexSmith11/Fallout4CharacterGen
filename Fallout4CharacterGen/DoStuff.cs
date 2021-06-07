using System;
using System.Collections.Generic;
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
            var randomIndexPerk = MyExtensions.GenerateRngInt(rng, perkList[randomIndexSpecial].Rows.Count);
            
            Console.Write(perkList[randomIndexSpecial].SpecialName + ": ");
            Console.WriteLine(perkList[randomIndexSpecial].Rows[randomIndexPerk].Name);
            
            
            // now need to match the two lists and merge them(?)
            await GeneratePerkLists(perkList, charactersSpecialRanks);
        }

        // refactor into SpecialManager
        /// <summary>
        /// generate a list of perks for the player, ordered by player level
        /// </summary>
        /// <returns></returns>
        public async Task GeneratePerkLists(List<SpecialSkill> perkList, List<SpecialSkill> character)
        {
            const int playerLevelCap = 80;  // make this changeable by user

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
            
            // new List<SpecialPerkRow> to contain all new perks - select ranges to display, don't need more than one list
            // for loop? where i = the player lvl
            // add a perk that:
            // - has a player req < i
            // - has special rank < SpecialRankRequirement of perk
            // - has to be either perk rank of 1 or rank of 2+ if there are already previous levels of same perk (1!)

            // (1!): just skip the perk.

            for (var i = 0; i < playerLevelCap; i++)
            {
                
            }
        }
    }
}
































