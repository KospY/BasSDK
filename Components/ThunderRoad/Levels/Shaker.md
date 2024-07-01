---
parent: Levels
grand_parent: ThunderRoad
---

# Shaker

The `Shaker` script is a component that can shake both the player camera as well as items within the zone. 

This component can be activated via an Event.

![Shaker][Shaker]

## Fields

| Field                       | Description
| ---                         | ---
| Zone                        | This is the [Zone][Zone] which the player/items need to be in in order to shake.
| Audio Source                | This audio plays when shaking 
| Startup Duration            | The duration of which the shaking starts. This will gradually lead to the maximum shake force.
| End Duration                | The duration of the maximum shake force.
| Items                       |
| Item Shake Min Max Force    | The minimum/maximum shake force (how much shaking happens.)
| Item Shake Interval         | The minimum/maximum shaking interval for items.
| Cone Angle                  | The angle of the item shaking in a cone.
| Direction                   | What direction the items shake in (1 = This direction)
| Ignore Items                | You can specify what items in the scene are not affected by the shaker. (Doesn't support [Item Spawners][ItemSpawner])
| Ignore Item Magnets         | You can specify what [Item Magnets][ItemMagnet] you can igore by the shaker
| Zero Drag Items             | Listed items will get zero drag when shaking
| Player                      |
| Player Camera Shake         | When ticked, the player in the zone's camera will shake. 
| Camera Shake Min Max Intensity | Minimum/Maximum intensity of the player camera shake.

## Events
[Unity Event Documentation](https://docs.unity3d.com/Manual/UnityEvents.html){: .btn .btn-purple }

| Event                       | Description
| ---                         | ---
| On Shake Begin()            | Plays the event when the shaking begins
| On Shake End                | Plays the event when the shaking ends

{: .tip}
This component can be activated via an event.

[Zone]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/Zone.md %}
[ItemSpawner]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/ItemSpawner.md %}
[ItemMagnet]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/ItemMagnet.md %}
[Shaker]: {{ site.baseurl }}/assets/components/Shaker/Shaker.png