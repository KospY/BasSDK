---
parent: Items
grand_parent: ThunderRoad
---
# Clothing Gender Switcher

{: .important}
This component must be placed on the same gameObject as your item

The Clothing Gender Switcher is a script which swaps the mesh of armor part items depending on what gender the player is. This is used for apparel items, which are used to equip armor and clothing.

![Clothing Gender Switcher Script][Inspector]

| Field | Description |
| :--- | :--- |
| maleModel | Male wearable item (Select item, not model) |
| mainMaleHandleRight | Main left handle of the male item. |
| mainMaleHandleLeft | Main right handle of item. |
| femaleModel | Female wearable item (Select item, not model) |
| mainFemaleHandleRight | Main left handle of the female item. |
| mainFemaleHandleLeft | Main right handle of the female item. |

This script also allows to swap [Handles][Handle] for the armor parts. If left blank, it will not change the handles on the object. 
For "Male Model" it will swap to the object referenced, and disable the "Female Model", if the player gender is set to Male, and vice versa. 

[Inspector]: {{ site.baseurl }}/assets/components/ClothingGenderSwitcher/Inspector.png
[Handle]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Handle.md %}