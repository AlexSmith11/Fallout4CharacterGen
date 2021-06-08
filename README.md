# Fallout4CharacterGen

Fallout 4 character generator

This is supposed to or going to be a basic RPG character generator.

Currently this generator will be used for Fallout 4, which has various ways to tailor your character.

The steps taken during this project:

- Web data scraper written to get the S.P.E.C.I.A.L. and perk data from the publicly available source at https://fallout.fandom.com/wiki
- Data formatting undertaken by me in Google Sheets to smooth out the data & produce a CSV file
- CSV Parser and JSON serialiser made to produce JSON files for future JavaScript web usage
- JSON deserialiser to read stored JSON
- Randomised player character S.P.E.C.I.A.L. generator
- Randomised player character valid perk generator

# Explanation

The two main ways a character is initially personalised is done is through the S.P.E.C.I.A.L. system, and the perk system.

S.P.E.C.I.A.L. stands for:

- Strength
- Perception
- Endurance
- Charisma
- Intelligence
- Agility
- Luck

These initial 'skills' contribute to the overall build of, and allow the player to roll play by creating
a framework for, their character. This framework is formed by allocating a set amount of points. The maximum
number you may use at the start in F4 is 28. Each S.P.E.C.I.A.L. skill has a max of 10 points.

For example: A build with stats of:

- Strength: 1
- Perception: 2
- Endurance: 2
- Charisma: 2
- Intelligence: 10
- Agility: 2
- Luck: 9
  Total: 28

Would be given to a character that would be considered clumsy, physically weak and socially inept, yet highly intelligent and lucky.

The second way, perks, are much more specific and can be assigned to characters throughout the playthrough with skill points.
These skill points are given to the player whenever a character levels up.
