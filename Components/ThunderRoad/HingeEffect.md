---
parent: ThunderRoad
---
# Hinge Effect

The Hinge Effect component plays an effect when a [HingeJoint] moves. 

```note
This component cannot reference a HingeDrive unless it is referenced at runtime, when the HingeDrive generates one.
```

## Component Properties

| Field                      | Description
| ---                        | ---
| Effect ID                  | This is the ID of the Effect that will play on Hinge Joint motion. This ID references the catelog ID of the effect.
| Min Torque                 | This is the minimum torque value of which the effect will play. Any number before this will not play the effect.
| Max Torque                 | This is the maximum torque value of which the effect will play. Any number after this will not play the effect.
| Joint                      | This references the HingeJoint that this effect will play to.

![Component]







[Component]: {{ site.baseurl }}/assets/components/HingeEffect/Component.png
[HingeJoint]: https://docs.unity3d.com/Manual/class-HingeJoint.html
[HingeDrive]: {{ site.baseurl }}{% link Components/ThunderRoad/HingeDrive.md %}