using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fallout4CharacterGen.DatabaseSource.Local;
using Fallout4CharacterGen.Interfaces;
using Fallout4CharacterGen.Middleware;
using Fallout4CharacterGen.Models;
using Fallout4CharacterGen.Utilities;

namespace Fallout4CharacterGen
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var application = new DoStuff();
            await application.CreateCharacter();
        }
    }
}