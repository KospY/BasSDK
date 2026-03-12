---
parent: Items
grand_parent: ThunderRoad
---
# Skill Tree Crystal

{: .important}
No matter what Visual Effect asset you use, it **must** have an Intensity float property to avoid errors!

The `SkillTreeCrystal` component is required to make a custom crystal core, used in the skill tree.

![Inspector]

| Field                             | Description
| ---                               | ---
| Glow Curve                           | The intensity curve this component uses to glow, evaluated over Time Glow Transition
| Time Glow Transition                 | The time it takes to cover the entire Glow Curve
| Time Vfx Transition                  | Controls the link Vfx `Intensity` property
| Time Vfx Transition Min              | The minimum link Vfx `Intensity` property's value for when two crystals are close
| Merge Vfx                            | The Visual Effect asset that plays when two crystals merge
| Merge Vfx Target                     | The target transform property for your merge vfx, this is positioned between each crystal
| Link Vfx                             | The Visual Effect asset that plays when two crystals are close
| Link Vfx Target                      | The target transform property for your link Vfx, this is positioned on the crystal it attempts to link to
| Tree Name                            | The name of your SkillTree
| Skill Tree Emission Color            | The emission colour of your SkillTree
| Override Crystal Colors              | If true, use this to override the colours defined on your material

[Inspector]: {{ site.baseurl }}/assets/components/SkillTreeCrystal/SkillTreeCrystal.png