---
parent: ThunderRoad
---
# Item Event Linker
*If you have not done so already, go read the [Event Linker][EventLinker]'s wiki page. This page only lists and explains the events available to this linker. It will not explain how to use the component itself.*

## Overview
The Item Event Linker listens for events emitted from a specific item.

A reference to an [Item][Item] component is required in order for this Event Linker to function.

## Events

| Event                                         | Description
| ---                                           | ---
| On Spawn(/Despawn)                            | Invoked when the item (spawns/despawns).
| On Snap(/Unsnap)                              | Invoked when the item is (placed/removed) from a holster/rack. 
| On Grab(/Ungrab)                              | Invoked when the item is (grabbed/ungrabbed) by a hand.
| On Drop                                       | Invoked when the last hand holding this item releases it.
| On Throw                                      | Invoked when the item is dropped with substantial force.
| On Grabbed Use (Press/Release)                | Invoked when the spell-cast/trigger button is (pressed/released) while the item is held.
| On Grabbed Alternate Use (Press/Release)      | Invoked when the alternate-use button is (pressed/released) while the item is held.
| On Non Grabbed Use (Press/Release)            | Invoked when the spell-cast/trigger button is (pressed/released) while the item is not held.
| On Non Grabbed Alternate Use (Press/Release)  | Invoked when the alternate-use button is (pressed/released) while the item is not held.
| On Tele Grab(/Ungrab)                         | Invoked when the item is (grabbed/ungrabbed) using telekinesis.
| On Tele Drop                                  | Invoked when the item is fully released by telekinesis
| On Tele Throw                                 | Invoked when the item is launched using telekinesis.
| On Tele Spin Start (Success/Fail)             | Invoked when the telekinesis spin action (succeeds/fails) to be started on the item.
| On Tele Spin End                              | Invoked when the item stops being spun using telekinesis.
| On Tele Repel (Start/End)                     | Invoked when the item (starts/stops) being pushed away using telekinesis.
| On Tele Pull (Start/End)                      | Invoked when the item (starts/stops) being pulled closer using telekinesis.
| On Fly (Start/End)                            | Invoked when the item has been thrown/launched.
| On Magnet (Catch/Release)                     | Invoked when an Item Magnet (catches/releases) this item.
| On Collision (Enter/Exit)                     | Invoked when the item (starts/stops) colliding with something regardless of damage dealt.
| On Harmless Collision                         | Invoked when the item collides with something with too little force to deal damage.
| On Damage (Dealt/Received)                    | Invoked when the item is involved in a collision that inflicts damage.
| On Kill Dealt                                 | Invoked when the item inflicts damage that kills a creature.
| On Consumed                                   | Invoked if the item is eaten by a creature.


[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/EventLinker.md %}
[Item]:  {{ site.baseurl }}{% link Components/ThunderRoad/Item.md %}