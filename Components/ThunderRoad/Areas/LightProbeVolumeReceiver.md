---
parent: Areas
grand_parent: ThunderRoad
---
# Light Volume Receiver

{: .note}
This documentation references [Light Probe Volumes][LightProbeVolume] and [Areas][Areas], and utilises both of these components to work. 

{: .note}
 [Items][Item] will automatically add this script when loading in to Dungeons. There is no need to add this component to items.

{: .warning}
Light Probe Volume Receivers will only work with mesh renderers that have a material and shader that supports 3D Volumes. In the Bas-SDK, this will be LitMoss and FabricAndFoliage. For your own shader, the 3D volume integration will be documented at a later date.

{: .tip}
This component can be placed on a parent of an object. It will add the light probe volumes on to all of the child renderers under it. For example, if you want to dynamically light all your vegetation, you can place them all under one parent gameobject, and place this script on to it to light all your vegetation. You can also place this at a prefab level and have one for each model. 

The Light Volume receiver is a component that utilises the Light Probe Volume on dynamicly lit objects. This allows you to light certain objects correctly without the need to use lightmaps (such as vegetation), and is required to light dynamic objects, such as creatures and items. This is a requirement in [Areas][Areas] with a [Light Probe Volume(s)][LightProbeVolume].

![Component][Component]

## Method
This field depicts what method the receiver uses to light the object. 

GPU instancing uses the GPU to light the object, so long as the object material utilises GPU Instancing. This will break SRP Batching.

SRP Batching uses the SRP Batcher to light the object. This is most recommended to use for objects lit by this receiver as most things will not be GPU Instanced. 

[SRP Batching Documentation](https://docs.unity3d.com/Manual/SRPBatcher.html){: .btn .btn-purple } [GPU Instancing Documentation](https://docs.unity3d.com/Manual/GPUInstancing.html){: .btn .btn-purple }

## Volume Detection

This field determinmes how the receiver detects its volume.

Dynamic Trigger utilises a trigger to detect the volume, whether it be a [Unity Events](https://docs.unity3d.com/Manual/UnityEvents.html) or through scripting methods. This is the more advanced detection method, however.

Static Per Mesh is a more utilised detection method, in which the volume will be detected per meth to apply the volume. 

## Misc

The field "Init Renderers On Start" is recommended to stay true. This is to ensure that the items are initialized to retrieve Light Probe Volumes on start, so they are not incorrectly lit on initialization of the game.

"Add Material Instance" is an important aspect of the receiver. This box makes the renderer have an instanced material, meaning it is a copy of the material per instance of the object. For example, if there is two items that share a material, the instance will make a copy material for each object so that they wont be incorrect when one material has a light volume change. This is recommended to be enabled to ensure that all game objects that utilise a Light Volume Receiver are lit correctly.

[Component]: {{ site.baseurl }}/assets/components/LightVolumeReceiver/Component.png

[LightProbeVolume]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/LightProbeVolume.md %}
[Areas]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/Area.md %}
[Item]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Item.md %}