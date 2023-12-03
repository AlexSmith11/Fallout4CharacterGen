using System;
using System.Threading.Tasks;
using Fallout4CharacterGen;

namespace Fallout4CharacterGen
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var userInterface = new DoStuff();
            await userInterface.CreateCharacter();
        }
    }
}