---
parent: ThunderRoad
---
# Whoosh Point

```note
Not to be confused with Whoosh Point in [ColliderGroup][ColliderGroup]

[ColliderGroup]: {{ site.baseurl }}{% link Components/ThunderRoad/ColliderGroup.md %}
```

The Whoosh Point is a transform which emits sound when the item is thrown or swung at a velocity. 

![Whoosh][Whoosh]

| Field                       | Description
| ---                         | ---
| Trigger                     | Depicts when the Whoosh sound plays. Options are "Always" (always will play sound when at velocity), on grab (Only plays when the item is grabbed) and On Fly (Only plays when the item is thrown or is flying).
| Min Velocity                | The minimum velocity required to play the sound effects.
| Max Velocity                | The maximum velocity required to play the maximum volume of the sound effects,
| Dampening                   | Depicts the speed of which the sound decays.
| Stop on Snap                | Depicts that the whoosh sound is to stop when held on a [Holder][Holder], such as a quiver or hips of the player.

It is recommended to place a whoosh on near the tip of the weapon, in the middle of a big prop, and if the item is long enough, another placed on the bottom of the handle. Multiple Whoosh colliders are not required, but not limited.


[Holder]: {{ site.baseurl }}{% link Components/ThunderRoad/Holder.md %}
[Whoosh]: {{ site.baseurl }}/assets/components/Whoosh/Whoosh.PNG