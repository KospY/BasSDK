---
parent: Areas
grand_parent: ThunderRoad
---
# Area

## Summary

The Area script is the main component for making dungeon rooms. This script is attached to the main parent of the room, and lets the game know where to spawn your new area.

When created, your area will create a `Boundary` around the renderers. This Boundary is important, as it connects rooms together. Please note, that the boundaries SHOULD sit on all [Area Gateway][AreaGateway] placed in the Area. To assist with bounds listed in the JSON, you can create a box collider to fit the room, and use it's scale/transform size for the JSON boundry.

## Component Properties

| Field | Description |
| --- | --- |
| Data ID | The Data ID is the ID of the Area JSON, which is required to spawn the area in a dungeon. |
| Lighting Preset Index | OBSOLETE: Do not worry about this. |
| Tool Boundary |  |
| Bound Include Inactive | When set to true/ticked, the bounds of the Area will also take inactive renderers into consideration. |
| Bount Include Disable on Play | When set to true/ticked, the bounds of the Area will also take renderers with the "Disable on Play" script. |
| Bound Renderer to Ignore | Listed objects are ignored by the automatic Boundary |
| Modified in Import | This is ticked when the room is locked. There is no need to edit this. |
| Culling |  |
| Spawn Room Objects Across Frames | Objects inside the area will spawn across a set amount of frames when spawning. The default is 2. |
| Audio Blend |  |
| Audio Sources to Blend | Listed audio sources will blend between themselves and other rooms. |
| Root No Culling | This will assign automatically when the room is locked. Ensure that the prefab contains a "RootNoCulling" game object when it is locked, as without it, the room will fail to load. |

![AreaScript][AreaScript]

### Events

The Area component has a number of UnityEvents that are invoked when the conditions are met.

[Unity Event Documentation](https://docs.unity3d.com/Manual/UnityEvents.html){: .btn .btn-purple }

| Event  | Description |
| --- | --- |
| On Player Enter | The event will play when the player enters the area. |
| On Player Exit  | The event will play when the player exits the area. |
| On Initialised  | The event will play when the room is initialised in the scene. |

### Buttons

This component can utilise buttons to assist in debugging, as well as creating your Area.

| Button  | Description |
| --- | --- |
| Static Batch  | *Internal Test Tool, not usable by user. |
| Active Collider   | When a room is locked in scene, all mesh colliders are disabled (this is enabled in game). This button, for testing, enables the mesh colliders. |
| Disable Mesh Collider | This button will disable the mesh colliders in the area. You can use the "Active Collider" to re-enable them. |
| Reset Import  |  *Internal Test Tool, not usable by user. |
| Add Selection to Bound Renderer Ignore  | When highlighting renderers, click this button to add it to the "Bound Renderer to Ignore" list. |
| Update Data Bounds | *Internal Test Tool, not usable by user. |
| Update Data Connection Position and Direction | *Internal Test Tool, not usable by user. |

[AreaScript]: {{ site.baseurl }}/assets/components/Area/AreaScript.png
[AreaGateway]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/AreaGateway.md %}