---
parent: Areas
grand_parent: Guides
---
# Setting up Level Data

- Create a level Data as other level Data. Setup the fields.
You can copy the Level_DungeonOutpost.
- In the module parts, you need to add or edit the LevelAreaModule :
    - areaSpawnDepth : 100 (option not used and not reliable set higher than the max number of area in the dungeon to avoid issues)
    - areaDespawnDepth : 100  (option not used and not reliable set higher than the max number of areas in the dungeon to avoid issues)
    - areaCullDepth : 2, the depth at which point the area will be culled out.
    - ListAreabyLenghtAndDifficulty : This is where you can choose the dungeon to spawn by Lenght and by Difficulty.
    It is a list of list
    first index length chosen, second index difficulty chosen.
    Data :
        - areaCollectionId : The Id of the dungeon or the fixlayout to spawn.
        - numberOfCreature : the number max creature to spawn it will try to split them equally in the different areas.
        - IsSharedNPCAlert : if the npc are allowed to share alert in the area.