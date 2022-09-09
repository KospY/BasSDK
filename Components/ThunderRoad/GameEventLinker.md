# Game Event Linker
*If you have not done so already, go read the [Event Linker][EventLinker]'s wiki page. This page only lists and explains the events available to this linker. It will not explain how to use the component itself.*

## Overview
The Game Event Linker listens for a number of common events emitted from the game itself.

## Events


| Event                             | Description
| ---                               | ---
| On Creature Spawn                 | Invoked when a creature enters the level.
| On Creature Healed                | Invoked when a creature receives health.
| On Creature Hit                   | Invoked when a creature takes damage.
| On Creature Kill                  | Invoked when a creature has been killed.
| On Creature Parry                 | Invoked when a creature parries an attack.
| On Creature Deflect               | Invoked when a creature deflects an incoming fireball.
| On Item Spawn                     | Invoked when an item is created.
| On Item Despawn                   | Invoked when an item is destroyed.

### From/To Filters

Using the From/To properties it is possible to only activate events under certain conditions.

For example, this configuration will only react to a hostile creature being killed by the player:
```
- Game Event: On Creature Kill
- From: Player
- To: Enemy NPC
```  

The following events are **not** affected by these filter properties. 
- On Creature Spawn
- On Item Spawn
- On Item Despawn



[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/EventLinker.md %}