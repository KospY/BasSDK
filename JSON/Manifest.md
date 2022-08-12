# Manifest

## Overview

Manifest files act as a descriptor for a mod, and are the first file read in a mod folder. 

They must be located at the top level of your mod folder, and are required for the game to recognize and load the rest of your files.


## Fields

| Field        | Description
| ---          | ---
| Name         | The name of your mod.
| Description  | A description of this mod.
| Author       | The creator of this mod.
| ModVersion   | The current version of this mod.
| GameVersion  | The game version. This value must be exact or the mod will not load. See the chart [below](#-game-version).



## Notes

### • Game Version

Mods with an incorrect or non-matching game version will be ignored. See the chart below for which values are for which game versions.

| Update | Version
| :---: | :---:
| u11 | 0.11.0.0
| u10 | 10.0
| u9 | 9.0
| u8 | 8.0
| u7 | 7.0
| u6 | 6.0


### • Deserialization
This file is deserialized internally using the **ModData** class.

[Vortex]: https://www.nexusmods.com/about/vortex/