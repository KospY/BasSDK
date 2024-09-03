---
parent: Items
grand_parent: Guides
---

# Items - Best Practices

When creating items, there are many practices to ensure that your items work effectively.

## Using the correct shader

{: .information}
For more information on the LitMoss shader, see the [LitMoss Info Page][LitMoss]

When creating your weapon, you should ensure that your weapon uses the same shader as all other weapons, as this both improves performance around shaders, as well as keeping all the shader-specific features functioning for your weapon. This includes features such as magic imbue and blood decals.

The shader that you should be using is `AsShader - LitMoss`. You will find this under `Shaders > ThunderRoad > AsShader-Litmoss`. 

If you don't know why your imbue works, check [Here][Item].

If you don't know why blood decals doesn't affect your weapons, check [Here][RevealDecal].

## Add the Parry Target

Making weapons isn't always for the player, as some can be used by the game's AI as well. That said, sometimes the Parry Target script is missing, which causes the AI to get confused as to what part of the weapon they can block with: so they dont.

To ensure that the AI correctly use your weapon, add the ParryTarget script to it, ensuring it scales to the length of the weapon.

For more information on the Parry Target, check [Here][ParryTarget].

## Add blunt damagers to your handles!

When creating weapons, it is best if all of the weapon does damage, even if it is a little bit. While your blade may slice and dice, your handle may not be set up in a way to deal damage properly to an enemy.

To ensure this, add a new collider group, name it something simple like "Handle" and add the handle colliders under it. Once done, add a new damager, name it "Blunt" and apply the handle collider group to it. Now, your weapon handle wont slide off enemies if you decide to hit them with it.

## Swap out Mesh Colliders for Primative ones

While having pin-point accurate colliders for your items is very fun and feels very precise, it is actually very expensive to do, and can cause mishaps with both collision performance and cause issues with collision accuracy, causing more instances of items going through items. Very precise colliders are sometimes also not easy to detect in-game, especially if your primative (that's box, capsule and sphere colliders) are somewhat accurate. 

For blades on weapons like daggers and swords, consider using capsule colliders. Although it'll look inacurate, functions very well with stabbing and slashing. Even vanilla weapons in Blade and Sorcery use capsule colliders for all bladed weapons.

Mesh colliders as a whole are quite expensive, let alone on items with rigidbodies, so it would be best for performance to switch to using primative colliders.

For more information on colliders, check out the [Unity Collider Documentation](https://docs.unity3d.com/Manual/CollidersOverview.html)

## Check your mesh

There are many ways of which the mesh of your weapon can be properly adjusted to best suit Blade and Sorcery. Usually, most meshes should work out of the box, however if you are experiencing issues with either performance or functions not working, check these options.
- Tri Count : Check the weapons tri count by clicking on its mesh or checking it in a 3D modelling software, as some models found online or created could be not "game-ready" and have millions of tris, whick can cause significant performance issues
- For best performance and functionality for your weapon, ensure the mesh doesn't have more than one material
- Alternatively, you can split the model in to different materials, but it would be best for it to be one mesh and one material

## Add more Handleposes

For some weapons, it is best that you are able to hold it in many ways, such as upside-down or any rounded rotation. By default, when creating a handle, a HandleLeft and HandleRight orientation is created by default. This means the player can only hold it upright in both hands, with no real freedom of any other rotations.

To fix this, you can copy rotations with little problems. Once you have set up the handle pose for the item, you can copy the left/right handles, rotate them 180 degrees on the Y axis to flip it to the other side of the weapon. Once done, you can then copy all the handleposes again, and rotate it 180 degrees on the Z Axis, to make it so you can hold it upside down.

If your item is similar at all angles, such as a flanged mace, then you can duplicate all handleposes again, and rotate it 180 degrees on the X axis. This means now that the player can now hold the weapon in 32 different angles (or 16 if your weapon is not mirrored at all angles). This allows fluid hand movement, while also removing the possibility of weapon "snapping" when trying to grab it at an unsupported angle.


 

[Item]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Item.md %}
[RevealDecal]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/RevealDecal.md %}
[ParryTarget]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/ParryTarget.md %}
[LitMoss]: {{site.baseurl }}{% link Components/Guides/Shader/LitMoss.md %}