# Zone

## Overview
Zones are a highly flexible tool for activating certain behaviours when an object or creature enter's its area of effect.

## Usage

### Basic Setup
1. Create an empty GameObject.
2. Select "Add Component" in the inspector window and choose any 3D collider to use as your Zone's area (Sphere Collider, Box Collider, Capsule Collider, etc).
3. Turn on "isTrigger" on the 3D collider component. 
4. Select "Add Component" in the inspector window and select the Zone component.

### Events
The Zone component has a number of UnityEvents that are invoked when something enters or exits its area of effect.   
For more about UnityEvents and how to use them, refer to the official [Unity Documentation][UnityEvents].

## Component Properties

| Field                       | Description
| ---                         | ---
| Invoke Player Exit On Awake | Causes the Player Exit event to be invoked immediately when the zone is loaded into the level.
| **Navigation**
| Nav Speed Modifier          | Enables the "Run Speed" effect.
| Run Speed                   | Sets the walking speed of the NPC creature that enters this zone.
| **Kill**
| Kill Player                 | If enabled, this zone will kill the player immediately on entering its area.
| Kill NPC                    | If enabled, this zone will kill any NPC creature that enters its area. 
| **Despawn**
| Despawn NPC                 | If enabled, this zone will despawn any **NPC** that enters its area, living or dead, after the time specified by the 'Despawn Delay' field.
| Despawn Item                | If enabled, this zone will despawn any **item** that enters its area after the time specified by the 'Despawn Delay' field.
| Despawn Delay               | Specifies an amount of time (in seconds) to wait before despawning the NPC/Item that enters the zone's area.
| **FX**
| Spawn Effect                | If enabled, this zone will spawn an [effect][Effect] when something enters its area, [positioned at the point of contact*](#-effect-spawn-position-behavior).
| Effect ID                   | The ID of the effect to spawn.
| Effect Mass Velocity Curve  | This curve maps the (mass * velocity) of the interactor at the moment of entry to the intensity of the effect.
| Effect Orientation          | The euler rotation of the spawned effect.
| **Teleport**
| Teleport Player             | If enabled, this zone will move any **player** that enters this zone to the target position.
| Teleport Item               | If enabled, this zone will move any **item** that enters this zone to the target position.
| Custom Teleport Target      | Specifies the transform that will act as a target point for.
| **Creature settings**
| Ignore Player Creature      | If enabled, this zone will not react to the player creature.
| Ignore Non Root Parts       | If enabled, this zone will only react to the root bone of creatures (hip bone).
| **Portals**
| Portals                     | A list of [Zone Portals][ZonePortal] to update when the player enters this zone.
| **Event**
| Player Enter Event          | Invoked when a player creature enters the Zone's area.
| Player Exit Event           | Invoked when a player creature leaves the Zone's area.
| Creature Enter Event        | Invoked when a creature enters the Zone's area.
| Creature Exit Event         | Invoked when a creature exits the Zone's area.
| Item Enter Event            | Invoked when an item enters the Zone's area.
| Item Exit Event             | Invoked when an item leaves the Zone's area.

## Notes

### • Event Invocation Time
Entry events are invoked at inconsistent times. The chart below depicts the order in which zone effects are applied before the event is invoked.

| Event           | Order
| ---             | ---
| Player Entry    | Kill > Teleport > Update Portals > **Invoked** > Spawn Effect
| Creature Entry  | **Invoked** > Spawn Effect > Kill > Despawn
| Item Entry      | Spawn Effect > Teleport > Despawn > **Invoked**


### • Effect Spawn Position Behavior
Zone effects will not spawn at the exact point of contact, rather the position will be approximated based on the what entered the zone. 

![][FXPosition]







[UnityEvents]: https://docs.unity3d.com/Manual/UnityEvents.html
[FXPosition]: {{ site.baseurl }}/assets/u11-modder-update-guide/Zone_fxposition.png
[ZonePortal]: {{ site.baseurl }}{% link Components/ThunderRoad/ZonePortal.md %}
[Effect]: {{ site.baseurl }}{% link Components/ThunderRoad/Effect.md %}