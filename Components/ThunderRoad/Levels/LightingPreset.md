---
parent: Levels
grand_parent: ThunderRoad
---

# Lighting Preset

The Lighting Preset Script is a component used to store lighting data on an Area or Level. This is not a requirement for levels, but is a requirement for Areas so that it can save the area lighting data (like sun direction, sun intensity, fog etc). 

To create this:
`Right click a space in the Project inspector > Create > ThunderRoad > Levels > LightingData`
![CreateData][CreateData]

[Unity Lighting Documentation](https://docs.unity3d.com/2020.2/Documentation/Manual/class-LightingSettings.html){: .btn .btn-purple }

This script does not change all lighting data fields. This means that the data in the Lighting Settings must be applied in the lighting settings rather than changed via this script.
This includes:
- Baked Global Illumination (needed for baking)
- Lighting Mode
- Progressive Updates
- Filtering
- Lightmap Resolution
- Lightmap Padding
- Max Lightmap Size
- Lightmap Compression
- Directional Mode
- Albedo Boost
- Lightmap Parameters

## Fields

{: .warning}
These fields will actively overwrite scene Lighting Settings. Make sure you make a backup of your scene before you apply these settings, incase you don't want to load your Lighting Settings.

| Field                       | Description |
| ---                         | --- |
| Ambient Intensity           | Sets the ambient intensity of the area/scene. |
| Shadow Color                | Sets the color of realtime shadows. |
| Baked Shadow Angle          | Changes the Baked Shadow angle, which adds artifical softening to edges of shadows. |
| Multiple Importance Sampling Tool | Adjusts the "Multiple Importance Sampling" section of the lightmapper.  |
| Direct Samples              | Sets the "Direct Samples" of the Lightmap Settings |
| Indirect Samples            | Sets the "Indirect Samples" of the Lightmap Settings |
| Environment Samples         | Sets the "Environment Samples" of the Lightmap Settings |
| Min/Max Bounces             | Sets the "Min/Max Bounces" section of the lightmap settings |
| Indirect Intensity          | Sets the "Indirect Intensity" section of the lightmap settings |
| AO Indirect Contribution    | Sets the Indirect Contribution section of Ambient Occlusion (will enable AO in baking.) |
| AO Direct Contribution      | Sets the Direct Contribution section of Ambient Occlusion (will enable AO in baking.) |
| Apply At Runtime            | When enabled, the directional light and its settings will be applied in game. This is recommended for exterior rooms, so that the directional light meets the baked directional light. If disabled, the realtime light will keep the same data as the previous area loaded. |
| Dir Light Color             | Will change the colour of the directional light when you enter this area. |
| Dir Light Intensity         | Will change the intensity of the directional light. |
| Dir Light Indirect Multiplier | Will change the indirect multiplier of the directional light |
| Directional Light Local Rotation  | Will change the direction/rotation transform of the directional light. |
| Fog                         | Will change the fog at runtime. <details>• *No Change* - Will not change fog from last area.<br>• *Disabled* - Will Disable Fog.<br>• *Enabled* - Will Enable Fog and apply the fog settings. </details> |
| Fog Color                   | Will change the fog color. |
| Fog Start Distance          | Will adjust the distance the fog will appear closest to the player |
| Fog End Distance            | Will adjust the distance the fog will be at max away from the player |
| Skybox                      | Will change the skybox material runtime. <details>• *No Change* - Will not change the skybox from the last area.<br>• *Disabled* - Will Disable Skybox, setting it to black.<br>• *Enabled* - Will Enable skybox and apply the material.</details> |
| Material                    | When skybox is enabled, the skybox will switch to this material. Through this, you can adjust the shader components of the skybox, being able to change the rotation, colour etc, without overriding the material. |
| Clouds                      | Adjust the clouds located in the Area scene.<details>• *No Change* - Will not change clouds from last area.<br>• *Disabled* - Will Disable Clouds.<br>• *Enabled* - Will Enable clouds and apply the settings. </details> |
| Cloud Softness              | Will change the softness/hardness of the clouds |
| Clouds Speed                | Will change the moving speed of the clouds | 
| Clouds Size                 | Will adjust the size/tiling of the clouds |
| Clouds Alpha                | Will adjust the transparency of the clouds |
| Clouds Color                | Will adjust the colour of the clouds. |



[CreateData]: {{ site.baseurl }}/assets/components/LevelData/Create.png