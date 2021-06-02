using System;
using System.IO;
using System.Threading.Tasks;

namespace Fallout4CharacterGen
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            await AskForFilename();
        }

        /// <summary>
        /// Loop user input until a valid filename is entered
        /// </summary>
        /// <returns></returns>
        private static async Task UserInputLoop()
        {
            var userInput = Console.ReadLine();
            
            if(CheckIfFileExists(userInput)) await LoadData(userInput);  // recursive condition
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
        private static async Task LoadData(string userInputSpecialType)
        {
            var data = CsvReader.LoadCsvData(userInputSpecialType);
            if (data is null)
            {
                Console.WriteLine("Please enter valid filename.");
                return;
            }
            JsonParser.WriteToDisk(data, userInputSpecialType);
            var deserializedSpecialPerkData = JsonParser.ReadSpecialSkillFromDisk(userInputSpecialType);

            var userInput = await AskForAnotherFilename();
            if (userInput == "y") await AskForFilename();
        }
        
        /// <summary>
        /// Ask the user for a file name
        /// </summary>
        /// <returns></returns>
        private static async Task AskForFilename()
        {
            Console.Write("Please enter the number of words you wish to view: ");
            await UserInputLoop();
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
        private static bool CheckIfFileExists(string specialType)
        {
            const string folderPath = @"C:\Users\alexs\Desktop\fallout4_csv\special\";
            var fileName = specialType + ".csv";
            var path = folderPath + fileName;
            
            return File.Exists(path);
        }
    }
}