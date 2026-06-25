---
parent: Event-Linkers
grand_parent: ThunderRoad
---

# Golem Event Linker
*If you have not done so already, go read the [Event Linker][EventLinker]'s wiki page. This page only lists and explains the events available to this linker. It will not explain how to use the component itself.*

## Overview
The Game Event Linker listens for a number of common events emitted from the game itself.

## Events


| Event                             | Description
| ---                               | ---
| On Golem Spawn                    | Invoked when the golem spawns.
| On Golem Wake                     | Invoked when the golem wakes.
| On Golem Melee Attack             | Invoked when the golem begins and ends a melee attack.
| On Golem Magic Attack             | Invoked when the golem begins and ends a magic attack.
| On Golem Deal Damage              | Invoked when the golem hits the player.
| On Golem Rampage                  | Invoked when the golem rampages.
| On Golem Crystal Break            | Invoked when a crystal on the golem is broken.
| On Arena Crystal Break            | Invoked when a crystal in the arena is broken.
| On Golem Staggered                | Invoked when the golem is staggered.
| On Golem Stunned                  | Invoked when the golem is stunned.
| On Golem Defeated                 | Invoked when the golem is defeated.
| On Golem Killed                   | Invoked when the golem is killed.

### Event Time Filters

Using the EventTime property it is possible to only activate events under certain conditions.

For example, if you only wanted to do something once the golem fully wakes:
- On Golem Wake (OnStart)
- On Golem Wake (OnEnd) This is when the event would be activated.

  
### Crystal Filters

Using the Crystals Left property it is possible to only activate events when a certain amount of crystals are left on the Golem's body.

[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/Event-Linkers/index.md %}