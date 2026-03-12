---
parent: Levels
grand_parent: ThunderRoad
---
# Golem Spawner

## Overview
This component allows you to spawn in a golem, either with [Unity Events](https://docs.unity3d.com/Manual/UnityEvents.html), or when the level loads.

## Properties
{: .Note}
All three Golem Wake animations are played at random once the player is seen by the Golem
| Field              | Description
| ---                | ---
| Golem Address         | ID of the golem. The golem in the vanilla game is `Bas.Boss.Golem`.
| Arena Crystal Randomizer        | The prefab of the Weak Point Randomizer component.
| Spawn on Start  | If enabled, the golem will spawn when the level loads.
| Action on Spawn        | Will change what the golem does on spawn <details>• *None* - The golem will spawn in and the player will need to go near it to wake it.<br>• *Disable* - The golem will spawn in with the GameObject disabled.<br>• *Wake* - The golem will spawn in and start attacking the player. The `Start Wake` events will still trigger.</details> |
| Events|
| On Start Wake Full        | When the golem wakes up. 
| On Start Wake Short A        | When the golem wakes up.
| On Start Wake Short B     | When the golem wakes up. 
| On Golem Awaken        | Once the wake animations finish or on spawn, dependent on your `Action on Spawn`
| On Golem Defeat        | When the golem loses all of its crystals.
| On Golem Kill        | When the golem gets its `Prismatic Crystal Core` removed by the player.
| On Golem Stun        | When the golem gets stunned by the player, typically by a `Meteor`.
| On Crystal Grabbed        | When the golems `Prismatic Crystal Core` gets grabbed by the player, either by hand or with telekinesis.
| On Crystal Ungrabbed        | When the golems `Prismatic Crystal Core` stops grabbed by the player, either by hand or with telekinesis.