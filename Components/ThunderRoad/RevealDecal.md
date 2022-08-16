# Reveal Decal

Reveal Decal is the script needed to spawn blood decals on weapons and props. The shader "ThunderRoad/Lit" is required for this, and requires a set up before Reveal starts working.

```tip
You can copy the material from the example weapons provided in the SDK, as they are set up with Reveal already.
``` 

![RevealScript][RevealScript]

You can use the buttons on this script to set the resolution of the reveal material.

```danger
It is recommended to keep the reveal resolution low to prevent the game to crash on `B&S Nomad`, or to cause a lag spike on `B&S PCVR`. It is recommended not to go above `512x512`, or `Mask Resolution Quarter`, to avoid any issues. You are unlikely to see a big change in resolutions above this point unless your item is very large. 
```

## Components

| Field                       | Description
| ---                         | ---
| Mask Width                  | Adjust the Width of the resolution of the Reveal Texture.
| Mask Width                  | Adjust the Height of the resolution of the Reveal Texture.
| **Type**
| Default                     | Applies to Weapons, Items, and non clothing itmes that require Reveal.
| Body                        | Applies to bodies/creatures. Reveal is removed on drinking Health Potions.
| Outfit                      | Applies to Armor. Reveal is not removed on drinking Health Potions.


## Material Setup

```note
The Textures required for this are located in SDK/Examples/Reveal
```

```tip
"Reveal Layers" in the material needs to be enabled to see this area of the material. Ensure that you untick it once it is finished.
```

![RevealMaterial][]

| Field                       | File Name
| ---                         | ---
| Layer Mask                  | Reveal_WeaponBlood_Mask
| Layer0 (R)                  | Revealed_WeaponBlood_c
| Layer0 (R) Normal           | `The Normal Map of your Material goes Here`
| Layer1 (G)                  | Revealed_Burn_c
| Layer1 (G) Normal           | Revealed_Burn_n

[RevealScript]: {{ site.baseurl }}/assets/components/RevealDecal/RevealDecal.PNG
[RevealMaterial]: {{ site.baseurl }}/assets/components/RevealDecal/RevealMaterials.PNG