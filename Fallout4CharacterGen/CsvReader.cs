using System;
using System.Collections.Generic;
using Fallout4CharacterGen.Models;
using Microsoft.VisualBasic.FileIO;

namespace Fallout4CharacterGen
{
    public static class CsvReader
    {
        public static List<CsvRow> LoadCsvData(string specialType)
        {
            const string folderPath = @"C:\Users\alexs\Desktop\fallout4_csv\special\";
            var fileName = specialType + ".csv";
            var path = folderPath + fileName;
            var specialSkill = new List<CsvRow>();

            using (var csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new[] {"#"};
                csvParser.SetDelimiters(new string[] {","});
                csvParser.HasFieldsEnclosedInQuotes = true;

                // skip the row with the column names if we have those (VERY USEFUL)
                // csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    var fields = csvParser.ReadFields();
                    if (fields?.Length == 0) return null;

                    var newRow = new CsvRow()
                    {
                        Name = fields[0],
                        SpecialRankRequirement = fields[1],
                        PerkRank = fields[2],
                        PlayerLevelRequirement = fields[3],
                        Description = fields[4],
                        FormId = fields[5]
                    };
                    specialSkill.Add(newRow);
                }
            }

            return specialSkill;
        }
    }
}