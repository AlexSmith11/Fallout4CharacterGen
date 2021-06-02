using System;
using System.IO;
using System.Threading.Tasks;

namespace Fallout4CharacterGen
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            await CsvAndJsonReaderWriter.AskForFilename(); // Run the CSV & JSON reader writer program
        }
    }
}