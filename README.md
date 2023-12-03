# Fallout4CharacterGen

Fallout 4 character generator

This is supposed to or going to be a basic RPG character generator.

This project was undertaken in order to learn about data handling. Transforming it, formatting it, loading/saving it, etc.

Currently this generator will be used for Fallout 4, which has various ways to tailor your character.

The steps taken during this project:

- Web data scraper written to get the S.P.E.C.I.A.L. and perk data from the publicly available source at https://fallout.fandom.com/wiki
- Data formatting undertaken by me in Google Sheets to smooth out the data & produce a CSV file
- CSV Parser and JSON serialiser made to produce JSON files for future JavaScript web usage
- Hosting of JSON on GitHub Pages
- JSON deserialiser to read stored JSON
- Randomised player character S.P.E.C.I.A.L. generator
- Randomised player character valid perk generator

TODO:

- Host output of the generator for a front-end to pickup
- Create a sorted for the list of perks. Currently they already are but what if they are not?  
- Creation of a frontend in React/NextJS. This will include ways to manipulate the data in various ways

S.P.E.C.I.A.L. hosted on: https://github.com/AlexSmith11/fallout4_data

# Gameplay Explanation

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
For example, if the character is level 18, they will have had 18 points to spend on perks.
