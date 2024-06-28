---
parent: Areas
grand_parent: ThunderRoad
---
# Light Probe Volume

{: .tip}
This documentation also document a script called "Light Probe Volume Generator". It is recommended to add the "Light Probe Volume Generator" component as it will add the Light Probe Volume as well.

The Light Probe Volume is an alternative to [Light Probes](https://docs.unity3d.com/Manual/LightProbes.html), and utilises Light Probes to generate 3D textures to light dynamic objects and objects not lit by lightmaps that have a [Light Volume Receiver][LightVolumeReceiver]. 

Please note that you need to place [Light Probes](https://docs.unity3d.com/Manual/LightProbes.html) inside the scene for the Light Probe Volume to retrieve 3D texture data. 

This script it primarily used in [Areas][Areas].

![Volume][Volume]

---
## Fields
---
### Size
The size of the Light Probe Volume can be edited via the "Edit Size" button, and also uses a box collider, which is required for the script to work. The button will update the box collider, however the box collider will naturally be a bit smaller than the volume itself.

### Priority

The Priority field determines which volume takes priority over lighting dynamic objects. For example, if you have many small light probe volumes, and one that fills the map, the volume with the lowest priority takes priority over the other.

### 3D Textures

When you bake lightmaps of the room, 3D textures will automatically be generated after the baking time. Ensure that the occlusion tickbox is enabled before you generate the 3D textures, as this 3D volume determines light behind walls, such as cover behind walls on a sunlight.

![3DVolumes][3DVolumes]

The component will generate four 3D textures. SH Ar (red channel), SH Ag (Green Channel), SH Ab (blue channel) and Occ (Occlusion). When baked, you can use the "Visualize Data Plane" button to view how the 3D volumes look on a plane, with the "Data Plane Axis Position" moving the 3D volume up and down in the preview. For this preview, ensure that gizmos are enabled.

![DataPlane][DataPlane]

### Texture Resolution

The texture resolution can be calculated depending on the size of the light probe volume using the "Calculate Texture Resolution" button. You can decrease the Total Texture Size of the Light probe volume by adjusting the texture res interval.

The mipmaps tickbox should be ticked, however this is stripped on Android.

{: .warning}
Do not use the "Create 3D Textures" button, instead, generate the 3D Texture volumes via a Lightmap Bake.

![TextureRes][TextureRes]

### Light Probe and BoxCollider Tools


These fields are separate from the 3D texture generation, and do not need to be touched.

The Lightprobe tool can place light probes on a grid to fill the box collider. This is an effective way to fill the box full of light probes, however this is not recommended as Light Probes which are placed inside objects may cause 3D volume artefacts and lighting errors.

The Box collider tools will automatically scale the box collider to the size of the light probe volume. The box collider will naturally be smaller than the light probe volume size. This is normal and will not cause any problems, the Light Probe Volume should be the size you want. 

### Misc Fields


The Light Probe Volume has a few visualizations to help debug and visualize the 3D volume. 

the "Visualize Intervals" shows the resolution of the 3D volume.

Visualize Data Plane is described under the 3D Textures heading, and is used to visualize the lighting of the dynamic objects in the area.

{: .note}
Since the visualizers are just Gizmos, this will not be visible in-game nor when Gizmos are disabled.

The layer is automatically set to "LightProbeVolume". This should not change.


[3DVolumes]: {{ site.baseurl }}/assets/components/LightVolume/3DVolumes.png
[DataPlane]: {{ site.baseurl }}/assets/components/LightVolume/DataPlane.png
[TextureRes]: {{ site.baseurl }}/assets/components/LightVolume/TextureRes.png
[Volume]: {{ site.baseurl }}/assets/components/LightVolume/Volume.png

[Areas]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/Area.md %}
[LightVolumeReceiver]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/LightProbeVolumeReceiver.md %}