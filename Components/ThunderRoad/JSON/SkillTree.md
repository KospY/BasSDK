---
parent: JSON
grand_parent: ThunderRoad
---

# Skill Trees

A Skill Tree JSON is used to create a new path of sorcery and is the base for all skills, crystal cores and spells that may belong to it.

| Keys | Description |
| --- | --- |
| id | The unique ID for the skill tree |
| version | Version of the JSON. Must be **0** |
| localizationGroupId | The ID used to reference translated text in the catalog |
| displayName | A localised name for the tree |
| description | A localised description of the tree |
| maxTier | The highest level reachable in this tree |
| showInInfuser | Boolean. If true, the tree appears as a selectable orb in the Skill map |
| color | The base RGBA color for the UI elements and the orb |
| emissionColor | The HDR intensity color. Values > 1.0 will be emissive |
| costMultiplier | Scales the skill point cost for every node within this tree |
| crystalItemId | The first Item ID of crystal core needed to unlock the tree. This item is spawned in the infuser |
| orbIconAddress | The Addressable address for the icon inside the orb |
| infuserTopParticleAddress | The particle effect that plays above the Infuser when this tree is selected |
| musicAddress | The background music that loops while this specific tree is open |
| videoAddress | The demonstration video shown in the Infuser UI |
| tierBlockers | A list of Skill IDs that must be purchased to unlock the subsequent tier |

---