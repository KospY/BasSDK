# Collider Group

The collider group script allows the item to deal damage to objects, as the [Damager](https://github.com/KospY/BasSDK/edit/gh-pages/Components/ThunderRoad/Damager.md) script is dependant of it. This component requires colliders attached to it to recognise what colliders utilise the damager.

This script also is a dependacy of imbuing weapons, and some fields require inputting for imbuement to work.

![](https://imgur.com/fRt0N4i)

## Generate Imbue Mesh Button

Before all components are filled in, it is recommended to create colliders for the collider group. For example, set up a collider group and colliders for the blade, a different collider group and colliders for the handle, etc. Ensure that these colliders are a child of the collider group.
Once all colliders are completed, you can click the "Generate imbue mesh". Once done so, a pink mesh will be generated and the "Imbue Effect Renderer" will be filled in. This mesh is used to 
