using System;
using Fallout4CharacterGen.DatabaseSource.Local;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen.Middleware
{
    public static class CharacterBuilder
    {
        public static void getCompanion(Character character)
        {
            var rng = new Random();
            var characters = Companions.CompanionNames;
            var index = rng.Next(characters.Count);
            character.Companion = characters[index];
        }

        public static void getHomeSettlement(Character character)
        {
            var rng = new Random();
            var settlements = Settlements.SettlementNames;
            var index = rng.Next(settlements.Count);
            character.HomeSettlement = settlements[index];
        }
    }
}