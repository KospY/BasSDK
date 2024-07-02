---
parent: Items
grand_parent: ThunderRoad
---
# Lever

The lever script allows you to create levers and [HingeDrive][HingeDrive] joints that utilise events at certain points.

{: .note}
Some of the fields used by Lever is a subsidary of [HingeDrive][HingeDrive]. Therefore, only "Lever" fields will be listed except for the "Angles config values", as there will be information on how events are played with these.

## Lever Value Fields

| Field                             | Description
| ---                               | ---
| Dead Zone                         | Allows the lever to have a "deadzone" where the angle of the "up" and "down" states is further away from the Minimum/Maximum angle
| Invert Output                     | Inverts "Up" and "Down"

![Component][Component]

## Events

[Unity Event Documentation](https://docs.unity3d.com/Manual/UnityEvents.html){: .btn .btn-purple }

{: .note}
This does not cover [HingeDrive][HingeDrive] events, only events used by Lever.

| Event                             | Description
| ---                               | ---
| Lever Up Event                    | This is the event that plays when the lever is pushed up, to the "Min Angle"
| Lever Down Event                  | This is the event that plays when the lever is pushed down, to the "Max Angle"
| Lever Analog Event                | This event plays during any analog event.

![Gizmo][Gizmo]






[Component]: {{ site.baseurl }}/assets/components/Lever/component.PNG
[Gizmo]: {{ site.baseurl }}/assets/components/Lever/gizmo.PNG
[HingeDrive]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/HingeDrive.md %}
