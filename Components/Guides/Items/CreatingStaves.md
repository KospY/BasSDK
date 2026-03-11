---
layout: default
title: Creating Staves
parent: Items
grand_parent: Guides
nav_order: 2
---

# Creating Staves
Staves are lightly modified items using unique [ColliderGroup]({{ site.baseurl }}{% link Components/ThunderRoad/Items/ColliderGroup.md %}) IDs and an extra reference. If you have not done so already, please read the [Creating Items Guide]({{ site.baseurl }}{% link Components/Guides/Items/CreatingItems.md %}) first.

## Contents

- [Collider Group IDs](#collider-group-ids)
- [Imbue Shoot](#imbue-shoot)

## Collider Group IDs

All [ColliderGroups]({{ site.baseurl }}{% link Components/ThunderRoad/Items/ColliderGroup.md %}) load [ColliderGroupData]({{ site.baseurl }}{% link Components/ThunderRoad/JSON/ColliderGroup.md %}) such as `BladeMace2h`, `PropDefault` or `BladeAxe`. Some Jsons have the `Slam` and `Use` imbue features, such as:
- CrystalStaff
- BladeSwordCrystal

As long as the colliderGroupId you reference in your Item Json has the correct imbue features, it will act as a staff.

![ColliderGroupID]

{: .tip}
It is recommended to use `CrystalStaff` as `BladeSwordCrystal` removes certain imbue effects. You could also make your own Json.

## Imbue Shoot

For an item to work like a staff you need a [ColliderGroup]({{ site.baseurl }}{% link Components/ThunderRoad/Items/ColliderGroup.md %}) with an Imbue Shoot reference assigned.

![StaffColliderGroup]

{: .important}
The blue arrow (Forward) of your Imbue Shoot transform should point **away** from your staff!

[ColliderGroupID]: {{ site.baseurl }}/assets/components/Guides/StaffGuide/ColliderGroupId.png
[StaffColliderGroup]: {{ site.baseurl }}/assets/components/Guides/StaffGuide/StaffShootPointColliderGroup.png
[StaffPosition]: {{ site.baseurl }}/assets/components/Guides/StaffGuide/StaffShootPointPosition.png