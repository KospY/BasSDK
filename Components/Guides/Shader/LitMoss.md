---
parent: Shader
grand_parent: Guides
---

# LitMoss 

{: .important}
This document is a work in progress.

ASshader - LitMoss is the shader we use for everything in Blade and Sorcery. This is located in `ThunderRoad > ASshader - LitMoss`.

This shader covers a lot of artistic features, such as Vertex painting, moss, detail and packed detail maps. 

This guide will also cover the MOES Texture Converter tool, as well as how to create MOES textures in Substance Painer. 

## Ocean Fog Caustics

These are modifiers that should not be changed. This is to ensure that ocean caustics are applied to the material. However, disabling these have never been tested, and therefore should not be changed.

## Reveal Layers

Reveal is what causes blood to be applied on items. 

To add blood on to weapons with this weapon, see [Reveal][Reveal].

{: .important}
Do not keep Reveal Layers enabled. Once your textures have been applied for reveal, untick the box. 

## Metallic (MOES/MOEDS/Moss ANSN)

There is a few options that are available when it comes to metallic features for LitMoss. 

### Moss Metal Mode
When enabled, it will force the Metallic map to be a MossMetal Mask instead. For weapons/props, you do not want this enabled, as it will break imbue.

### MOES and MODS

Mode allows you to swap between MOES and MODs:
- Mode 0 - MODS (Metallic, AO, Detail, Smoothness)
- Mode 1 - MOES (Metallic, AO, Emission, Smoothness)

The order of MOES/MODS goes in order of RGBA for packed textures. You put your MOES/MODS map inside the `Metallic Map` field. Ensure that it is Mode 1 for weapons, so they accept weapon imbue. 

{: .note}
For imbue to be supported, `Use Emission` does not need to be supported.

For a video guide on making a MOES texture in Substance Painter, see [this Tutorial](https://youtu.be/H4-o27IbeGM)

#### Using the Moes Maker

The Moes Maker is a simple tool used to convert your textures in to a MOES map. It will combine a metallic, ambient occlusion, emission and smoothness in to one single packed texture. This conversion tool also supports Roughness maps.

![MoesMaker][MoesMaker]

- Base Map : Put your colour map/BaseMap in to this field.
- Metallic : Put your metallic map or metallic smoothness map here
- Occlusion : Put your Ambient Occlusion/AO Map here
- Emission : Put your emission map here. If your emission map doesn't exist/is not used, put the emission source to "Black" for it to not affect the material, white for it to affect all of the material, or texture if you do have an emission texture for the material.
- Smoothness: A smoothness texture doesn't usually exist, it is usually embedded with the metallic texture. However, you can insert your roughness texture here, and tick "Invert Smoothness" if you do.

Once your textures have been submitted, press "Combine Textures" and it will create a MOES map for your material.

![MOESTexture][MOESTexture]

Once complete, you now insert this in to the "Metallic Map" field.

This is the fields that are set for weapons, and with these settings set, Imbue should work.

![WepMat][WepMat]


[Reveal]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/RevealDecal.md %}

[MoesMaker]: {{ site.baseurl }}/assets/components/Guides/Shader/MoesConvert.png
[MOESTexture]: {{ site.baseurl }}/assets/components/Guides/Shader/MOESTexture.png
[WepMat]: {{ site.baseurl }}/assets/components/Guides/Shader/WepMat.png