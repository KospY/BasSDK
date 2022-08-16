# Imbue Controller

```note
Not to be confused with [ImbueZone][ImbueZone]

[ImbueZone]: {{ site.baseurl }}{% link Components/ThunderRoad/ImbueZone.md %}
```

Imbue Controller is used to imbue a specific weapon in scene, or the [Item][Item] that it is attached to.

![ImbueController][ImbueController]

## Components

| Field                       | Description
| ---                         | ---
| Imbue Group                 | References [ColliderGroup][ColliderGroup] that is to be imbued. Note that the collidergroup has to be an imbueable ID in the JSON for this to work.
| Imbue Rate                  | Incidcates the Rate/Speed of imbuing the weapon when enabled.
| Imbue Max Percent           | Indicates the maximum percentage that this componant can imbue noted collidergroup. 
| Imbue Spell ID              | Imbues the [Item][Item] with specific spell ``(Vanilla: `Fire`, `Gravity` and `Lightning`)``. Accepts Modded Spells.


[ColliderGroup]: {{ site.baseurl }}{% link Components/ThunderRoad/ColliderGroup.md %}
[Item]: {{ site.baseurl }}{% link Components/ThunderRoad/Item.md %}
[ImbueController]: {{ site.baseurl }}/assets/components/ImbueController/ImbueController.PNG