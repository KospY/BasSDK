# Creature Event Linker
*If you have not done so already, go read the [Event Linker][EventLinker]'s wiki page. This page only lists and explains the events available to this linker. It will not explain how to use the component itself.*

## Overview
The Creature Event Linker listens for events emitted from a specific creature.

A reference to a [Creature][Creature] component is required in order for this Event Linker to function.  
As of u11.2, this can only be done through custom scripting or by creating a custom creature.

## Events

| Event                             | Description
| ---                               | ---
| On Despawn                        | Invoked when the creature despawns from the scene.
| On Heal                           | Invoked when the creature receives health.
| On Damage                         | Invoked when the creature is damaged.
| On Kill                           | Invoked when the creature has been killed.
| On Resurrect                      | Invoked if the creature has been resurrected.
| On Fall                           | Invoked when the creature stops standing.
| On Touch (Start/End)              | Invoked when any object collides with the creature and deals damage.
| On Touch No Damage (Start/End)    | Invoked when any object collides with the creature and deals no damage.
| On Grab(/Ungrab)                  | Invoked when the creature is (grabbed/released) by another creature.
| On Total Ungrab                   | Invoked when the last hand releases the creature.
| On Tele (Grab/Ungrab)             | Invoked when the creature is (grabbed/released) using telekinesis.
| On Total Tele Ungrab              | Invoked when the last telekinesis holder releases the creature.
| On Tele Repel (Start/End)         | Invoked when the creature (starts/stops) being pushed away using telekinesis.
| On Tele Pull (Start/End)          | Invoked when the creature (starts/stops) being pulled closer using telekinesis.
| On Ragdoll (Pre/Post) Slice       | Invoked (before/after) a limb or head has been severed.
| On Melee Attack (Start/Finish)    | Invoked when the creature (begins/completes) a melee attack.
| On Ranged Attack (Start/Finish)   | Invoked when the creature (begins/completes) a ranged attack.
| On Spell Attack (Start/Finish)    | Invoked when the creature (begins/completes) a magic attack.


## Alive State Filter
Using the Alive State property it is possible to only activate events under certain conditions.

| State                             | Description
| ---                               |  ---
| Either                            | The event will activate regardless of the creature's state.
| Only Alive                        | The event will only activate while the creature is alive.
| Only Dead                         | The event will only activate while the creature is dead. 



[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/EventLinker.md %}
[Creature]:  {{ site.baseurl }}{% link Components/ThunderRoad/Creature.md %}