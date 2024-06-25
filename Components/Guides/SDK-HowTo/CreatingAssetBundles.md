---
parent: SDK-HowTo
grand_parent: Guides
---
# Using SDK to create asset bundles

## Prerequisites

You will need Unity installed and the BAS SDK downloaded and opened in Unity. You can follow [Unity SDK Setup][SDKSetup] to do so.

---

In this tutorial, we will see how to create asset bundles, packages that can be read by the game and contain assets that can be used by mods (textures, sounds, meshes, etc…).

Asset management is managed by [Unity Addressables](https://docs.unity3d.com/Packages/com.unity.addressables@0.8/manual/index.html), it provides an easy way to load assets by “address”.

The first time you open the SDK, we recommend opening the asset bundle builder `ThunderRoad (SDK) → Asset bundle builder` and the addressable group windows `Window → Asset management → Addressable -> Groups`

<video src="/assets/components/Guides/AddressableBundle/AssetBundleSetup.mp4" width="880" height="440" controls></video>


## Addressable groups

---

This window shows all the assets that are referenced in different groups.

![AssetBundles][AssetBundles]

By default, 6 groups are already configured. 

`Default`, `Fonts`, `ShadersHD`, `ShadersAndroid,` and `Shaders`, are system groups that are already packed as asset bundles into the game executable. **These groups should never be modified in the SDK**, and are only used as a reference so Unity will not pack assets that are already available in the game (avoid duplication in memory). 

The `ProtoAssets`group is an already setup group that serves as an example for doing a mod (it contains a small dungeon, a test level, a character, a helmet, and some prototype items)

The process to create a new group and assign assets is the following:

1. Create a new group with `New → Packed assets`
2. Drag and drop your asset in the group
3. Set a unique address for it, we recommend using an address like `[Your Mod Name or Author Name].Item.XX` to avoid conflicts with other mods or default assets
4. Assign a label, either `Windows` and/or `Android`. If you are asset is used on both platforms, use both labels. 

<video src="/assets/components/Guides/AddressableBundle/NewAssetBundle.mp4" width="880" height="440" controls></video>


{: .tip}
With labels, it’s possible to define two different assets with the same address, and set each one with a different label. This way, it’s possible to dynamically change the asset used depending on the platform (It’s handy in case you want to use low-quality assets on Android and high quality on PCVR).

{: .warning}
When building bundles, groups that don’t use `localBuildPath` and `LocalLoadPath`as paths will be ignored (like the system groups). So it’s important to be sure that your new groups are using these paths. You can check how the `protoAssets` group is configured as an example.
![localbuildpath][localbuildpath]

## Asset bundle builder

---

The asset bundle builder is a custom tool used to create asset bundles from the addressable groups.

If not already open, go to `ThunderRoad (SDK) → Asset bundle builder` to open it.

![BundleBuilder][BundleBuilder]

The tool should list every **Asset Bundle Group** assets in the project. By default, only one is created: `Proto`

**Asset Bundle Group** are a scriptable object containing a list of Addressable Asset Groups. This way, it’s possible to build a mod containing multiple addressable asset groups.

### Create a new Asset Bundle Group

To create a new **Asset Bundle Group**, you can right-click anywhere in the project window, and select `Create → ThunderRoad → Editor → Asset Bundle Group`

A new **Asset Bundle Group** will be then created and can be edited to include any Addressable group you need.

<video src="/assets/components/Guides/AddressableBundle/CreateBundle.mp4" width="880" height="440" controls></video>

To edit an **Asset Bundle Group**, double-click on it in the Asset Bundle Builder or find it in the project and click on it.

![EditGroup][EditGroup]

- The folder name is the name of your mod folder. This name is important and can’t be changed after building the asset bundle (asset bundle catalog json reference this path)
- The mod manifest part is an option you can check so that when exporting the bundles, it will automatically generate the manifest file. For more information about the manifest see [JSON Modding][JSONModding]

### Build bundles for PCVR

To build the bundles, select the Asset Bundle Group you want to build and press “Build asset bundle group(s) selected”

![BuildBundle][BuildBundle]

If you didn’t check any export, the bundles will be generated in the path: `BuildStaging\AddressableAssets\Windows` or `BuildStaging\AddressableAssets\Android`, depending on the current SDK platform selected.

{: .note}
In case you checked export, you will need to provide the Game Executable Path and then the bundles will be automatically copied to the game mod folder, with the Manifest file (if configured in Asset Bundle Group), and corresponding JSON files

{: .note}
If any folder with the same name as the Asset Bundle Group Folder name exist `BuildStaging\Catalogs\default` , the files will be automatically copied with the assets in the destination mod folder.

### Build bundles for Android (Nomad)

To build Android bundles, change the platform to Android going to `File → Build Settings → Select Android → Switch Platform`

![BuildSettings][BuildSettings]

![AndroidBuild][AndroidBuild]

Once you changed to the Android platform, the Asset Bundle Builder should be slightly different, without the game executable option.

In this mode, using the export function will automatically push the assets into the device if connected. You can test the connection using the “Check Android device connection”, that will list all devices connected. 

![ExportMod][ExportMod]

{: .tip}
It’s possible to run the game with custom arguments on both PCVR and Android. Useful for loading directly into a custom map (Example: `-level arena`) 
For more information about arguments, see [Launch parameters](Launch%20parameters%2072f2cadc31eb49b3834611f688c53e8f.md)

{: .note}
If “check android device connection” and “run/stop game” buttons throw an exception error in the console, go to `Edit → Preferences → External tools` and check / uncheck Android SDK path. This seem to fix the issue with Unity being unable to find the SDK path.

![AndroidTools][AndroidTools]



[SDKSetup]: {{ site.baseurl }}{% link Components/Guides/SDK-HowTo/UnitySDKSetup.md %}
[JSONModding]: {{ site.baseurl }}{% link Components/Guides/SDK-HowTo/JSONModding.md %}

[AssetBundles]: {{ site.baseurl }}/assets/components/Guides/AddressableBundle/AssetBundles.png
[localbuildpath]: {{ site.baseurl }}/assets/components/Guides/AddressableBundle/localbuildpath.png
[BundleBuilder]: {{ site.baseurl }}/assets/components/Guides/AddressableBundle/BundleBuilder.png
[EditGroup]: {{ site.baseurl }}/assets/components/Guides/AddressableBundle/EditGroup.png
[BuildBundle]: {{ site.baseurl }}/assets/components/Guides/AddressableBundle/BuildBundle.png
[BuildSettings]: {{ site.baseurl }}/assets/components/Guides/AddressableBundle/BuildSettings.png
[AndroidBuild]: {{ site.baseurl }}/assets/components/Guides/AddressableBundle/AndroidBuild.png
[ExportMod]: {{ site.baseurl }}/assets/components/Guides/AddressableBundle/ExportMod.png
[AndroidTools]: {{ site.baseurl }}/assets/components/Guides/AddressableBundle/AndroidTools.png


[AssetBundleSetup]: {{ site.baseurl }}/assets/components/Guides/AddressableBundle/AssetBundleSetup.mp4
[NewAssetBundle]: {{ site.baseurl }}/assets/components/Guides/AddressableBundle/NewAssetBundle.mp4
[CreateBundle]: {{ site.baseurl }}/assets/components/Guides/AddressableBundle/CreateBundle.mp4