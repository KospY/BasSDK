---
parent: FAQ
grand_parent: Guides
---

# Android / Nomad Specific FAQ

This page contains information about Blade and Sorcery Nomad, regarding porting mods over to Nomad, as well as best practises for mods that you will port over. This will contain information about what runs well on Nomad, as well as components that will cause issues on the port.

{: .note}
B&S Nomad will sometimes be explained as "Android" as it is the Android-specific build of Blade and Sorcery

## What is B&S: Nomad?

Blade and Sorcery: Nomad is a version of Blade and Sorcery built for and inclusive to the Android platform. This includes stand-alone headset versions like the PICO 4 and the Quest 2. There are some features which are missing from Blade and Sorcery Nomad that are in the PCVR version, such as some Dungeon rooms, as well as the Citadel map.

## How much is Blade and Sorcery: Nomad?

$19.99 / £14.99. For region-specific prices, check the store page [Here](https://www.meta.com/en-gb/experiences/blade-sorcery-nomad/2031826350263349/).

## What headsets is B&S:Nomad available on?

The Quest 2, The Quest 3, The Quest Pro and the PICO 4.

## When does 1.0 release on Nomad?

There is no planned release date, but the planned release period is Q3 2024.

## Why does Blade and Sorcery: Nomad look worse than PCVR Blade and Sorcery?

Because B&S:Nomad is for the stand-alone headsets, it means that it needs to run on those platforms effectively at the best performance that we can make it. This means that a lot of refactoring needs to take place, such as reducing texture resolution, android varients of all VFX, stripping of LOD0 models, and so forth. Because of this, B&S:Nomad looks a bit different to the PCVR version, as well as some functionality being absent on Nomad, such as post-processing and bloom.

## Do we downgrade the PCVR version for Nomad?

No, Nomad is a separate, standalone, built-for-Quest version of Blade and Sorcery for people who have no PC to play PCVR. Nomad is made specifically for Quest hardware.

When it comes to Nomad, we always prioritise getting PCVR working and then port the PCVR version to make Nomad. This includes making exclusive prefabs/models, VFX and custom scene adjusting to ensure that it runs well on Quest hardware. On Nomad, we might strip certain assets, like vegetation and set dressings, while on PCVR they will cause no issue; we never allow Nomad development to influence PCVR development.

## How do we optimise Blade and Sorcery for Nomad?

There are many points we do to make Nomad work effectively as a standalone port. Since standalone ports are mobile, we know that it will not be powerful enough to run the full PCVR version. Here are some of the things we do to make sure Nomad runs decently:
- If a model has LODs, we strip the LOD0 model and use the LOD1 instead
- Utilise Baked LOD groups, ensuring that some models have the same Lightmap UVs for their LODs and saving lightmap space by using the same lightmap space for the LOD1, LOD2 etc
- Strip shader keywords that may cause issues (such as LOD Crossfade)
- Create Android-specific rooms, scenes, creatures and armors
- Replace all VFX with Android varients (the Android headsets do not support VFX Graph very well, so we utilise Shuriken particles instead)
- Add LODs to as many models as we can
- Custom culling zones for heavy scenes
- Manually adjust texture resolutions (4k textures are expensive in memory!)
- Change the LitMoss shader and Ocean/River shader to its Android varient (this is done in the shader, it is not a separate shader!)
- Remove or reduce as much transparency as possible
- Strip expensive assets, such as heaps of vegetation, expensive models like roots, and unnecessary set dressing (done via the "StripOnAndroid" tag, the Android Exporter script or the "Strip or Disable Relative to Platform" script)
- Disable heavy realtime lights
- Switch from Shadowmask Lightmaps to Subtractive Lightmaps
- Reduce the maximum amount of enemies in areas/waves that are spawned in at a time
- Reduce physics quality so the CPU can keep up with physics interactions (Nomad's physics quality is PCVR's "Low Quality" physics setting)



