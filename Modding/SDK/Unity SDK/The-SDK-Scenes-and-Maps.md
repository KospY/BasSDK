# Scenes and Maps

_This is a work in progress by Drags, so some content may be missing or out of date_
# Scenes

The SDK contains an example scene called "Protolevel". We will be using this as an example to show what you can do for levels.

![](https://i.imgur.com/E9J0Xwf.png)

Below, you will see all available tools that allow you to create your level!

# Level Definition
Level definition is the only mandatory component in the scene. It define the starting position of the player, if the player body must be spawned and allow referencing some transforms that can be used later from level modules.

![](https://i.imgur.com/8xkMvia.png)

The "Player Start" part of this component is necessary. To do this, create a GameObject. Name it "PlayerStart" (Optional) and then drag it on to the Player Start area. Now, move that PlayerStart gameobject to the bottom of where the your scene is, as it will be the location the player will spawn when you load the map. Remember, the Blue Arrow ALWAYS points forward. If the blue arrow always points to the same location, look at the bar, and you should see "Global". Change that to "Local" and it should work correctly.

# Render cube map
This component allow the creation of the preview orb on the world map.

![](https://i.imgur.com/0wNiSbq.png)

To use it you need to place it in the desired position and press "Create level preview"

![](https://i.imgur.com/mJPoQsP.png)

Once this is completed, you should find a material and CubeMap file. If you click on the .cubemap file, you will be able to see its components. If you got to the bottom right you should see the example of what you will be able to see on the map. To change the rotation of this cubemap, you can drag it to alter its rotation.

![](https://i.imgur.com/buwbPup.gif)

# Wave selector
The wave selector lets the player spawn waves of enemies. This can be found in Assets/SDK/Prefabs/Maps

![](https://i.imgur.com/r4BXgtY.png)

If you click the dropdown on the wave selector, you can see the component "WaveSelector". On this, you should be able to see the components needed to fill in. 

![](https://i.imgur.com/TuyNpmp.png)

* The ID is the component stated in "Waves" json. It allows the game to see what waves are situated to what level, and allow the user to create custom waves for their maps
* Menu Address should not be changed.
* Spawn Location is another game component labelled below. 

Spawn Location is another script that needs to be added to allow NPCs to spawn. 

![](https://i.imgur.com/UAGhWhQ.png)

You need to create a separate GameObject containing this script. Then, put other gameobjects as a child on to it. Move the GameObjects to locations where the NPC will spawn, similarly to the player spawn. Blue will always point forward, meaning that the NPCs will spawn facing the direction the blue arrow is pointing. Create multiple of these, then in the Spawn Location script, move the children to the List. 
Notes: 
* It's possible to create multiple wave selector with different spawn points and waves.
* AltarSandBook or AltarGreyBook prefabs can be used to set a wave selector (include an altar and book mesh).

# Item selector
UI Item Selector is used to display the item selector menu.
Spawn point is the reference of the transform used to spawn the item.

![](https://i.imgur.com/yIGbox2.png)



Notes: 
* Item will spawn relative to their holder point.
* ItemSelector prefab can be used to set an item selector (include a book mesh).

# World map
UI World Map is used to display available levels.
* The parameter `mapId` in the level JSON is used to reference the ID set on this component. 
* Locations are the parent transform of a list of transforms representing the available locations on the map. 
* The parameter `mapLocation` in the level JSON reference the `locations` child index directly (so transforms order).
* Canvas Detail reference the transform used to show level detail.

![](https://i.imgur.com/g10QiIP.jpg)

Notes: 
* DefaultMap prefab is the same as the one in the home (so you can use the prefab to check the position index).
* It's possible to create new maps and put them in a dedicated level that can be accessed from the default map in the home, or directly from a custom home (total conversion mods).
* Home map can be changed in Game.json (`levelHome` parameter)

# Mirror
The Mirror component and prefab is used to display a mirror. The "Zone" box collider which you can edit defines the area where the mirror will work. If the player is outside that zone, the mirror will deactivate.

![](https://i.imgur.com/yL054Ef.png)

# Pages viewer
The `PageViewer` prefab show text and image of the page group ID provided.
New pages can be setup by appending text JSON (see `pageGroups` property)

![](https://i.imgur.com/TeW7ouC.png)

# Weapon rack
The `WeaponRack` prefab is the same rack used in-game. It contain already configured holder definitions.
Checking 'Player Rack' will spawn the saved player weapons on it.

![](https://i.imgur.com/PzaY4UF.png)

# Ropes
Rope Definition component are used to create ropes. It derive from Handle Definition and automatically generate a rope in game from the start and end position provided. Parameters should be self explanatory, and you are able to move the end to allow you to change the rotation and length of the rope itself.

![](https://i.imgur.com/bOifJoG.png)

Notes: 
* The prefabs `RopeHorizontal` and `RopeVertical` can be used to create rope quicker.
* Vertical and horizontal prefabs use different handle value (see JSON), so try to use the one suited for your needs.

# Ladder builder
The `Ladder Builder` component allow you to quickly generate ladders from rung prefabs.

![](https://i.imgur.com/j8SXf6B.png)

# Item Spawner
The `Item Spawner` component allow you to spawn an item not available from the SDK (barrels, crates, weapons, etc...)

![](https://i.imgur.com/8BEq0PM.png)

Notes: 
* If your mod contains some item prefabs, you can directly drop them in the scene.
* If you place them in the scene, in the Item Definition "Spawn on Load" MUST be ticked. This must be the case for every Item Definition stored in the level.

# Creature Spawner
The `Creature Spawner` component allow you to spawn a creature with parameters that have content from JSON files you have created or that is already in the base game. Along with this, you can also set a delay for spawning.

![](https://i.imgur.com/zOuMCeD.png)

# Audio Mixer linker

Attach this component to any audiosource to automatically link it to the game mixer in game (to make sound options working as well as slowing time effect). Put this on an Audio Source to allow the players to adjust the sound volume in game via sound settings.

![](https://i.imgur.com/iXUPfyD.png)

# Zone
Zone component are used to trigger specific actions when the player or an NPC enter it's attached collider. The collider must be set to "Trigger" also, to make sure that the zone works correctly.
The zone can teleport the player, delete items, kill NPCs and the player, spawn effects, as well as have Events occur via scripts or adjust game values. 

![](https://i.imgur.com/NOLOlBg.png)

The enter event is when the player enters the zone, and the exit event is when the player exits the zone. It allows you to adjust many components. For example, enable or disable a gameobject

![](https://i.imgur.com/JtBZikh.png)

This would mean that when the player enters the zone, the GameObject will disappear/disable, and will reappear/enable when the player enters the zone again. 