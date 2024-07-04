---
parent: Misc
grand_parent: ThunderRoad
---

# Material Instance

{: .important}
This component requires a collider to be put on the gameObject before adding the script.

{: .note}
This component does not have any fields.

Material Instance is a component primarily used on objects that have unique material changes in game. This script is automatically added to any Mesh Renderer that is affected by a [Light Volume Receiver][LightVolumeReceiver], so long as "Add Material Instance" tickbox is checked.

This component is mainly used to change certain sections of a material, while making sure not to edit all objects that use that material, such as [Light Probe Volumes][LightProbeVolume] and [Reveal][Reveal].

For example, if there is two items that share a material, the material instance will make a copy material for each object so that they wont be incorrect when one material has a light volume change.

[Reveal]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/RevealDecal.md %}
[LightVolumeReceiver]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/LightProbeVolumeReceiver.md %}
[LightProbeVolume]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/LightProbeVolume.md %}