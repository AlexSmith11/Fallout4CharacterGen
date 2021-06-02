using System;
using System.IO;
using System.Threading.Tasks;

namespace Fallout4CharacterGen
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            // Run the CSV & JSON reader writer program
            await CsvAndJsonReaderWriter.AskForFilename();
        }
    }
}