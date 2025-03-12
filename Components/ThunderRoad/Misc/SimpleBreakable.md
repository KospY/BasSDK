---
parent: Misc
grand_parent: ThunderRoad
---

# Simple Breakable

The "Simple Breakable" script is a component that is a simpler version of [Breakable][Breakable]. This component still has the needed fields such as health and minimum hit velocity/mass, without having more advanced features such as breakpoints and Explosion forces.

![SimpleBreakable][SimpleBreakable]

## Fields

| Field                             | Description
| ---                               | ---
| Allowed Damage Types              | List of potential types of damage the simple breakable can take
| Max Health                        | The maximum health the breakable can have before it is broke
| Collission delay                  | The minimum delay between the collisions with this item being registered
| Min Hit Velocity                  | The minimum velocity this item can take before it takes damage
| Min Hit Mass                      | The minimum mass an item can be before it can deal damage to the breakable
| Velocity Curve                    | The hit velocity curve of min to max damage
| Mass Curve                        | The curve of min to max mass an item needs to be to deal damage
| Damage Curve                      | The curve of the damage that can be dealt to this item

## Events

[Unity Event Documentation](https://docs.unity3d.com/Manual/UnityEvents.html){: .btn .btn-purple }

| Event                             | Description
| ---                               | ---
| On Restore                        | Event plays when the breakable item is restored to its original form
| On Damage                         | Event plays when the breakable item is damaged
| On Break                          | Event plays when the breakable item is broken

[SimpleBreakable]: {{ site.baseurl }}/assets/components/Breakables/SimpleBreakable.png
[Breakable]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Breakable.md %}