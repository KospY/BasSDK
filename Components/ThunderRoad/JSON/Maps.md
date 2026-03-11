---
parent: JSON
grand_parent: ThunderRoad
---

# Maps

A map JSON is used to add a new page to the map board in the Home/Shop.

| Keys | Description |
| --- | --- |
| id | The unique ID for the map |
| version | Version of the JSON. Must be **0** |
| worldmapPrefabAddress | The Addressable address of the worldmap prefab, this is the gameObject with every location defined |
| worldmapTextureAddress | The Addressable address of the worldmap texture (e.g., **Eraden**) |
| worldmapLocalisationId | A localized name for the location (e.g., **EradenMap**) |
| bookmarkIconAddress | The Addressable address for the bookmark below the map |
| bookmarkColor | The colour of the bookmark icon |
| iconColor | The colour of the icon |
| order | The order of the bookmark on the map |