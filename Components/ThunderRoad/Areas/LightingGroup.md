---
parent: Areas
grand_parent: ThunderRoad
---

# Lighting Group

[Areas and Dungeon Documentation][Areas]{: .btn .btn-blue }

Lighting Group is a neccessary script for creating Dungeon Rooms/Areas. This script will reference a Lighting Preset for your room, of which then it will fill in all the mesh renderers, light volumes and lightmaps, once the room has been baked. This component contains all lighting data of the room, so that it can properly bake it, as well as load/unload it between rooms. 

This component needs an [AreaLightingGroupLiteMemoryToggle][AreaLightingGroupLiteMemoryToggle] to function properly

This component can also be used in levels, but it isn't neccessary.

![Component][Component]

[Areas]: {{ site.baseurl }}{% link Components/Guides/Areas/index.md %}
[Component]: {{ site.baseurl }}/assets/components/LightingGroup/Component.png
[AreaLightingGroupLiteMemoryToggle] {{ site.baseurl }}{% link Components/ThunderRoad/Areas/AreaLightingGroupLiteMemoryToggle.md %}