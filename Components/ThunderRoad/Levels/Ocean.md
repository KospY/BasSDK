---
parent: Levels
grand_parent: ThunderRoad
---
# Ocean

## Overview

This component will create an ocean inside your level. The height of the ocean is determined by the **Y position** of the object holding this component.

{: .warning}
The object holding this component should not be rotated in any way. Doing so will create holes on the surface of the ocean mesh.

{: .warning}
The object holding this component should not be moved after the level has loaded. Doing so will change where the game thinks the ocean is, but not move the visual representation, causing players to drown in air. 


## Component Fields

| Field                         | Description
| ---                           | ---
| Prefab Address                | The ocean prefab to use when ocean quality is set to **high**.
| Low Quality Prefab Address    | The ocean prefab to use when ocean quality is set to **low**.
| Low Quality                   | A custom low-quality ocean asset to use. This should *not* be a prefab.
| Show Only When In Room        | If this ocean is part of a dungeon room, enabling this will have it only render when the player is within the room.

## Ocean Prefab Addresses
Any of the following addresses can be entered into the `Prefab Address` and `Low Quality Prefab Address` fields to change the appearance of the created ocean. 

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/components/Ocean/Ocean_Preview.mp4" type="video/mp4">
</video>

#### High-Quality
`Bas.Ocean.Citadel`  
`Bas.Ocean.Greenland.CharSelection`  
`Bas.Ocean.Greenland.Lighthouse`  
`Bas.Ocean.Greenland.Outpost`  
`Bas.Ocean.Greenland.LostCove`  
`Bas.Ocean.Greenland.Zipline`  
`Bas.Ocean.Greenland.WatchTower`  


#### Low-Quality
`Bas.Ocean.LowQuality`  
`Bas.Ocean.LowQuality.Interior`  
`Bas.Ocean.LowQuality.LostCove`  
`Bas.Ocean.Home.LowQuality`  
`Bas.Ocean.Citadel.LowQuality`  