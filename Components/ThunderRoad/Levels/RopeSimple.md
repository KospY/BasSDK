---
parent: Levels
grand_parent: ThunderRoad
---
# Rope Simple

## Overview
This component allows you to hang physics objects from a rope. You can link this rope to any position in your level, or another physics object using the `Connected Body` property.   

This component makes use of Unity's built-in [Spring Joint][SpringJoint] component.

![][SampleImage]

### Setup
The object you wish to hang must have a **RigidBody** component on it.
1. Create a new object and make it a child of the object you wish to hang.
2. Select `Add Component` in the Unity inspector and add the RopeSimple component to your new object.
3. Create a new object and drag it to the position your rope will attach to in the world.
4. Assign this new object to the `Target Anchor` field.


## Component Properties

| Field                       | Description
| ---                         | ---
| Target Anchor               | A transform referencing the position that this rope will link to.
| **Spring Joint**
| Spring                      | The force used to keep the rope at its maximum length.
| Damper                      | How much force should be applied to slow the ropes swinging.
| Min Distance                | How low the distance can be between the hanging object and the anchor point.
| Max Distance                | How great the distance can be between the hanging object and the anchor point (relative to starting point).
| Connected Body              | The rigidbody linked to this ropes spring joint. [^1]
| **Rope Mesh**
| Radius                      | Controls the radius of the generated rope mesh.
| Tiling Offset               | The vertical tiling to be applied to the generated rope mesh's material.
| Material                    | The material to apply to the generated rope mesh.
| **Audio**
| Effect Id                   | The EffectData ID to use for this rope. [^2]
| Audio Min Force             | The lower range for the force used when calculating the effect intensity.
| Audio Max Force             | The upper range for the force used when calculating the effect intensity.
| Audio Min Speed             | The lower range for the speed used when calculating the effect intensity.
| Audio Max Speed             | The upper range for the speed used when calculating the effect intensity.


## Notes

### • Audio Effect
When the SimpleRope's rigidbody is not [sleeping][RBSleeping], it will play the effect referenced by `Effect Id`.  
The intensity of this effect is calculated by the amount of force stretching the rope, multiplied by the velocity of the hanging object's rigidbody. 

----

[^1]: This property is used only when linking your rope to another rigidbody. Otherwise you may leave this field blank. 
[^2]: The base-game effect ID for this is `RopeSqueak`.



[SpringJoint]: https://docs.unity3d.com/ScriptReference/SpringJoint.html
[RBSleeping]: https://docs.unity3d.com/Manual/RigidbodiesOverview.html
[SampleImage]: {{ site.baseurl }}/assets/components/RopeSimple/sample.png