# Collider Group

The collider group script allows the item to deal damage to objects, as the [Damager][Damager] script is dependant of it. This component requires colliders attached to it to recognise what colliders utilise the damager.

This script also is a dependacy of imbuing weapons, and some fields require inputting for imbuement to work.

![Script Preview][SPreview]

## Generate Imbue Mesh Button and the Imbue Effect Renderer

Before all components are filled in, it is recommended to create colliders for the collider group. For example, set up a collider group and colliders for the blade, a different collider group and colliders for the handle, etc. Ensure that these colliders are a child of the collider group.
Once all colliders are completed, you can click the "Generate imbue mesh". Once done so, a pink mesh will be generated and the "Imbue Effect Renderer" will be filled in. This mesh is used to tell the particle systems of imbuing where to go, and to identify the imbuement particles when it is imbued.

For the Imbue Effect Renderer, you can either use this generated mesh, or create your own.

![Imbue Generated Mesh][ImbueGenMesh]
```warning
The mesh on the right is to show what the mesh looks like, do not duplicate and put it to the side and reference this one.
```

```tip
You must disable the pink mesh by clicking on it, and unticking the "mesh renderer" checkbox.
```

> ![Turn off Mesh Renderer][MeshRenderer]

## Imbue Emission Renderer

The Imbue Emission Renderer is where you reference the mesh that will show the emission once the item is imbued. When referenced, ensure that the emission color is black, as if emission is already another color, it will be overwritten by the imbue emission. 

```warning
It is recommended that there is one mesh per collider group. Do not reference a mesh in more than one collider group, this is not supported.
```

## Imbue Shoot

The Imbue Shoot transform is to depict where spells shoot from if the weapon is set up like a magic staff. The Z Axis / Blue Arrow points forward as to where the projectile appears from

```tip
The Magic Staff Collider group reference is referenced in the Item JSON. This will be useless if not set up so.
```

## Whoosh Point

This is usually automatically generated when you play the game. However, if it feels like your imbue trails/sounds/haptics aren't feeling right, you can apply it exactly like the item's [Whoosh Point][Whoosh].

## Imbue Custom Fx Controller

This is your [FX Controller][FXController] if you want to make a custom FX for your imbue. For example, the Torch uses this component to spawn Fire effects when it is imbued.

## Imbue Custom Fx ID

This is needed to spawn the Imbue Custom Fx Controller, where the ID is the spell that can trigger this FX. (e.g. Fire, Gravity, Lightning)

[Damager]:      {{ site.baseurl }}{% link Components/ThunderRoad/Damager.md %}
[SPreview]:     {{ site.baseurl }}/assets/components/ColliderGroup/CollidergroupScript.png
[ImbueGenMesh]: {{ site.baseurl }}/assets/components/ColliderGroup/ImbueMeshGen.PNG
[MeshRenderer]: {{ site.baseurl }}/assets/components/ColliderGroup/ImbueMeshDisable.PNG
[Whoosh]:       {{ site.baseurl }}{% link Components/ThunderRoad/WhooshPoint.md %}
[FXController]: {{ site.baseurl }}{% link Components/ThunderRoad/FxController.md %}