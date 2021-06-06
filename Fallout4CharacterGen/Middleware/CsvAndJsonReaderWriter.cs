using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fallout4CharacterGen.DatabaseSource.Local;
using Fallout4CharacterGen.Interfaces;
using Fallout4CharacterGen.Models;

namespace Fallout4CharacterGen.Middleware
{
    public class CsvAndJsonReaderWriter : ICsvJsonReaderWriter
    {
        /// <summary>
        /// inefficient method that returns all special type perks
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<SpecialSkill>> GetSpecialSkillData()
        {
            var allSpecialSkillsList = new List<SpecialSkill>();
            var specialNames = new List<string>
                {"strength", "perception", "endurance", "charisma", "intelligence", "agility", "luck"};

            foreach (var specialName in specialNames)
            {
                var tempSpecialPerkList = JsonParser.ReadSpecialSkillFromDisk(specialName);
                var specialSkillsList = new SpecialSkill
                {
                    Rows = new List<SpecialPerkRow>(),
                    SpecialName = specialName
                };

                foreach (var perk in tempSpecialPerkList)
                {
                    specialSkillsList.Rows.Add(perk);
                }

                allSpecialSkillsList.Add(specialSkillsList);
            }

            return allSpecialSkillsList;
        }

        /// <summary>
        /// Ask the user for a file name
        /// </summary>
        /// <returns></returns>
        public async Task AskForFilename()
        {
            Console.Write("Please enter the name of the file you wish to parse: ");
            await UserInputLoop();
        }

        /// <summary>
        /// Loop user input until a valid filename is entered
        /// </summary>
        /// <returns></returns>
        private async Task UserInputLoop()
        {
            var userInput = Console.ReadLine();

            if (CheckIfCsvFileExists(userInput)) await WriteCsvDataToJson(userInput); // recursive condition
            else
            {
                Console.Write("Your input was not a number - please try again: ");
                await UserInputLoop();
            }
        }
        
        /// <summary>
        /// Load CSV data, parse it into JSON, then read JSON back in.
        /// </summary>
        /// <param name="userInputSpecialType"></param>
        /// <returns></returns>
        private async Task WriteCsvDataToJson(string userInputSpecialType)
        {
            var data = CsvReader.LoadCsvData(userInputSpecialType);
            if (data is null)
            {
                Console.WriteLine("Error");
                return;
            }

            JsonParser.WriteToDisk(data, userInputSpecialType);

            var userInput = await AskForAnotherFilename();
            if (userInput == "y") await AskForFilename();
        }

        /// <summary>
        /// Ask the user for another filename
        /// </summary>
        /// <returns></returns>
        private static async Task<string> AskForAnotherFilename()
        {
            Console.Write("Would you like to parse another file? y/n: ");
            return Console.ReadLine();
        }

        /// <summary>
        /// Check for files existence
        /// </summary>
        /// <param name="specialType"></param>
        /// <returns></returns>
        private static bool CheckIfCsvFileExists(string specialType)
        {
            const string folderPath = @"C:\Users\alexs\Desktop\fallout4_csv\special\";
            var fileName = specialType + ".csv";
            var path = folderPath + fileName;

            return File.Exists(path);
        }
    }
}