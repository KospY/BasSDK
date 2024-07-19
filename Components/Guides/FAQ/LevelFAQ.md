---
parent: FAQ
grand_parent: Guides
---

# Level FAQ

{: .tip}
Question not here, or any of the solutions did not work? Ask for help in the [Blade and Sorcery Discord](https://discord.gg/atdUuvd6).

## How do I make Levels/Maps for Blade and Sorcery?

A video guide for making levels will be uploaded at a later date. In the meantime, you can join the [Discord](https://discord.gg/atdUuvd6) and look at the #modding-resources forum to follow the community-written guides.

## Why can't I spawn in my map? / My map kicks me back to character selection

- Make sure that your level has a [Level][Level] component, as well as a [PlayerSpawner][PlayerSpawner] component, where the ID of the PlayerSpawner is set to "default".
- Make sure that your scene does not have any enabled cameras. This can either cause an issue with sound/UI, or force it to be the main camera. 
- Ensure that your Level JSON is set up correctly, with the correct game-modes set. You can find information on this in the [Level JSON][LevelJSON] documentation.

## My map isn't on the map selection board

- Ensure that your Level JSON is set up correctly, with the correct game-modes set. You can find information on this in the [Level JSON][LevelJSON] documentation.
- Ensure that your map's ID is not conflicting with another existing map.
- Ensure that your scene addressable address is correct
- Ensure that your worldmapTextureAddress is correct.
- Ensure that "showOnMap" in the JSON is set to true
- If the problem is on Nomad, ensure that "hideOnAndroid" is set to false
- Ensure that "showOnlyDevMode" is false

## No enemies spawn when starting a wave

- Ensure that your WaveSpawner book has a [WaveSpawner][WaveSpawner] script linked to it.
- Ensure that your NPC spawn points are located on a navigation mesh
- Ensure that you have baked a Navigation Mesh or baked a NavMeshSurface.

## My Wave book is empty

- Ensure that the ID located in the Wave Spawner's "setup" area is set to a map that already exists (like Arena), or the wave json have the ID set correctly in the "waveSelectors". The easiest function is to set the ID in the UI Wave Spawner script to a level that already exists.
- Your SDK may be installed incorrectly or is out of date. Try a re-install of the SDK and make sure you are on the Unity version 2021.3.38f1

## My Zones don't work, or they block the player.

- Ensure that the zone is set to the "Zone" layer
- Ensure that the zone has a collider, and that the collider is set to "trigger". 


[Level]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/Level.md %}
[PlayerSpawner]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/PlayerSpawner.md %}
[LevelJSON]: {{ site.baseurl }}{% link Components/ThunderRoad/JSON/Level.md %}
[WaveSpawner]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/WaveSpawner.md %}