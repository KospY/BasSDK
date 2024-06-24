---
parent: Areas
grand_parent: Guides
---
# Setting up Dungeon Data

The dungeon is entirely made with catalog Data, AKA JSONs.

The dungeon is a list of areas chosen randomly, connecting to each other. There can not be a loop or a split path in the dungeon, so each area cannot have more than 2 connections.

### Area Tables

The dungeon uses an AreaTable to make the path. Consider an Area Table as a group of Areas that should randomly spawn at a certain depth of the dungeon.

If you want to add your Area to the Greenland Dungeon from the game, you need to find the AreaTable from the game data and copy it, then edit it to add your custom Area. We use a different table so find the one where you want to add your area.

If you want to create your own dungeon, you must Create your own AreaTable. You can copy-paste the data from the proto and edit them.

It is recommended to have a different AreaTable so you can specify your dungeon path. It's best to at least have an Area Table for Start Area, one for the End Area, and one for the area(s) in between. 

AreaTable Is a List of Drops. You can add Drops for each area you want in this table.

Set the fields for each Drops (Area) :

- **bpGeneratorIdContainer:**
    - dataId : The Area ID
    - Category : Area (it can be something else but for now just put Area)
- **exitConnection:** 
For each connection in the Area, if it can be an exit, add its index to the list.
- **entranceConnection:**
    
    For each connection in the Area, if it can be an entrance add its index to the list.
    
- maxCreature : 
The maximum number of creatures to spawn in this area when a dungeon spawns.
To have a creature in the Area, the area needs to have CreatureSpawner in the prefab.
**Note**: The number of creature spawners in the area can be higher, then it will spawn the number chosen and randomly pick a creature spawner for each creature that needs to be spawned. 
If the number is higher than the creature spawner then it will only spawn a creature for each creature spawner (which will result in fewer creatures than expected).
- probabilityWeight : A weight to change the probability of this area being chosen.

### AreaCollectionDungeon

Note that to have different lengths with level parameters you need to set an AreaCollectionDungeon for each star you want.

- Under the data folder, create a folder called AreaCollectionDungeons if there is none. Under this folder create or copy the AreaCollectionDungeon JSON.
- Set the ID for your dungeon.
- In “Path” add the AreaTables in the order you want for your dungeon.
For Each edit data :
    - numberAreaMinToSpawn / maxToSpawn : number of areas to pick from this table in a row.
    - creatureFillPercentage : 0 to 1, will multiply the max number of creatures set in the table (use 0 if you want no creatures)
    - lootFillPercentage : ***Not used/Not yet implemented***
    - isSharedNPCAlert : if true, when an NPC finds the player, all the other creatures in the area will be alerted.
    - areaPoolIdContainer : dataId set the AreaTable id you want to pick the area from.
- Set Fields :
    - retry_previous_area_allowed : number of areas redraw allowed to create dungeon path. Augment if too many fail.
    - bounds_margin : 0.1, the distance allow area to have Area bound intersection.
    - BackupList : a list of Areas that generates if the retry previous area amount has exceeded, and the generation still fails.