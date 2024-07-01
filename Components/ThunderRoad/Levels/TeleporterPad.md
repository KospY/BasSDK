---
parent: Levels
grand_parent: ThunderRoad
---

# Teleporter Pad

The Teleporter Pad script is a component that can levitate the player and play Events.

[Unity Event Documentation](https://docs.unity3d.com/Manual/UnityEvents.html){: .btn .btn-purple }

This component requires a [Zone][Zone] to activate, and had a lot of customizationable options for the player levitation. This can also utilize a [Event Load Level][EventLoadLevel] to load a level after the event. 

{: .note}
The "On Teleport()" event plays after the end of the teleporting duration and not during.

![Component][Component]

## Fields

| Field                       | Description
| ---                         | ---
| Activate Zone               | Reference the zone that the teleporter will activate when the player is inside.
| Levitation Target           | Reference an empty transform of which the player root will raise to when the teleporter is activated.
| Startup FX Controller       | Reference the [FX Controller][FXController] that plays when the teleporter event starts up.
| Lock FX Controller          | Reference the [FX Controller][FXController] that plays when the player is locked in place after the startup event.
| Teleport FX Controller      | Reference the [FX Controller][FXController] that plays when the teleporter event ends.
| Parameters                  |
| Startup Duration            | How long the teleporter event takes to start up, and place the player at the Levitation Target.
| Teleporting Duration        | How long the teleporting sequence at the end of the startup takes, levitating the player at the Levitation Target.
| Teleporting Flash Duration  | How long the teleport "flash" takes. This brightens the screen to full white.
| Levitation Force Curve      | The curve of how much force is applied to the player during levitation/startup event.

## Levitation Target

The levitation target teleports the player's root (Feet) to the specified gameObject. The player will levitate to this position, and stay there for the amount of time "Teleporing Duration" is set to.

![TPTarget][TPTarget]


[Zone]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/Zone.md %}
[FXController]: {{ site.baseurl }}{% link Components/ThunderRoad/Effects/FxController.md %}
[EventLoadLevel]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/EventLoadLevel.md %}
[Component]: {{ site.baseurl }}/assets/components/TeleporterPad/Component.png
[TPTarget]: {{ site.baseurl }}/assets/components/TeleporterPad/TPTarget.png
