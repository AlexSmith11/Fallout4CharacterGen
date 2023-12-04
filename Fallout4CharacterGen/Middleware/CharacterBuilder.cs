using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fallout4CharacterGen.DatabaseSource.Local;
using Fallout4CharacterGen.Interfaces;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen.Middleware
{
    public class CharacterBuilder: ICharacterBuilder
    {
        private readonly ICsvJsonReaderWriter _readerWriter = new CsvAndJsonReaderWriter();
        private readonly ISpecialManager _specialManager = new SpecialManager();
        
        public async Task<Character> BuildCharacter()
        {
            var character = new Character
            {
                Special = await GetSpecialAndPerks(),
                Companion = GetCompanion(),
                HomeSettlement = GetHomeSettlement(),
                Name = await _readerWriter.AskForCharacterName()
            };


            return character;
        }

        private async Task<List<SpecialSkill>> GetSpecialAndPerks()
        {
            var allPerks = await _readerWriter.GetAllSpecialPerks();     //TODO: Refactor into the generation of the characters perks below?
            var charactersSpecialRanks = await _specialManager.GenerateSpecialPoints();

            var characterWithPerks = await _specialManager.GeneratePerkLists(allPerks, charactersSpecialRanks);
            if (!characterWithPerks.Contains(null)) return characterWithPerks;
            Console.WriteLine("Perk generation error");
            return null;

        }
        
        private static string GetCompanion()
        {
            var rng = new Random();
            var characters = Companions.CompanionNames;
            var index = rng.Next(characters.Count);
            return characters[index];
        }

        private static string GetHomeSettlement()
        {
            var rng = new Random();
            var settlements = Settlements.SettlementNames;
            var index = rng.Next(settlements.Count);
            return settlements[index];
        }
    }
}