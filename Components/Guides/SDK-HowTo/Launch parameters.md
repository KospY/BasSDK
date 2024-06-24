---
parent: SDK-HowTo
grand_parent: Guides
---
# Launch parameters

The game can be launched with some parameters, below is the list of available commands:

## **Loading a specific level**

---

`-level <level name>`

Examples:

> BladeAndSorcery.exe -level arena
> 

## Skip intro (Warpfrog logo)

---

`-skipintro`

## Level option

---

`-leveloption <level option>`

Configure a level option. Below is the list of different option available:

- DungeonLength
- Seed
- Difficulty
- DungeonRoom
- PlayerContainerId
- PlayerSpawnerId

Each option must end with `=<value>`

Examples:

> BladeAndSorcery.exe -level dungeonoutpost -leveloption DungeonLength=1 -leveloption Seed=1234
> 

## **Set player height (Imperial or Metric)**

---

`-playerheight <height>`

Examples:

> BladeAndSorcery.exe -playerheight 185
> 

> BladeAndSorcery.exe -playerheight 6’4
> 

## **Set player character slot**

---

`-charindex <character index>`

Select a specific character to use (when loading directly on a level)

Values are 0,1,2,3,4…

## Force a Mod to load

---

`-mod <mod folder name>`

## **Force calibration**

---

`-forcecalibration`

Force calibration of the player when the level is loaded.

## **Force default spectator mode**

---

`-spectator <spectator mode>`

Force a default spectator mode for the monitor view. By default, the HMD view is selected, but it is possible to change to a better-quality spectator view at the cost of some performance (so it is recommended for good configuration only). This is the same as clicking on the different view buttons on the arcade panel.

List of spectator mode:

- fpv (first person view)
- free (a spectator camera that we can control using mouse and keyboard keys)
- auto (experimental, camera that will automatically change point of view and follow the player)

Examples:

> BladeAndSorcery.exe -spectator fpv
>