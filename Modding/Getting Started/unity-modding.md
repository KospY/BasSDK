# Unity Modding

We are going to create a mod called `MyMod`. It will have a single prefab which will get packed into an addressable bundle. Along with the `catalog` json files required for the mod to load.

## Prerequisites

You will need Unity installed and the BAS SDK downloaded. You can follow the [Unity and SDK Setup Guide][SDKSetup] to do so.

## Creating our mod folder structure

It is a good idea to create a folder for your mod to keep all the things it needs together.

In the Unity editor `Project` tab, create a folder under `Assets` called `MyMod`, along with other folders like `Prefabs`, `Models`, `Materials`, `Addressable` to help you organise your mod.


![Mod Folders][ModFolders]

## Create Asset Bundle Group

Our mod `MyMod` needs an `Asset Bundle Group`.

This is what tells our mod builder which asset bundles should be packaged together for our mod.

Lets create one for our mod.

1. Right Click our `Assets/MyMod/Addressable` folder we created previously.
2. Go to ThunderRoad
3. Go to Editor
4. Click `Asset Bundles Group`
5. Name it `MyModGroup`
6. Click on `MyModGroup`
7. Go to the `Inspector` tab in the Unity editor.
8. In the `Folder Name` type `MyMod`.
9. Click the + icon under `Addressable Asset Groups` to add a new entry.

```warning
Make sure you don't name your Asset Bundles Group the same thing as your mod folder, as this will conflict with our Addressable Asset Group in later steps. Appending "group" to the name is good practice, like MyModGroup.
```

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/getting-started/unity-modding/asset-bundle-group.mp4" type="video/mp4">
</video>


We have now created an Asset Bundle Group called `MyModGroup`.

This tells the mod builder what our mod folder is called and which `Addressable Asset Groups` will be included in it.


## Addressable Asset Group

The `Addressable Asset Group` is what will hold references to our assets so they can be loaded into game. 

```tip
You can split your mod up into multiple groups in order to separate maps, weapons and their materials and effects for example. So if you only change a weapon, you only need to rebuild the weapons group rather than a single large one which contains multiple weapons and maps.
```

### Opening the Addressables Group tab

The `Addressables Group` window lets us add new Addressable groups, setup labels and edit the addressable addresses for our assets.

The `Addressables Group` window can be opened by going to:

`Window` menu > `Asset Management` > `Addressables` then clicking `Groups`

![Addressable Group][AddressableGroupMenu]

It will open a window on your Unity editor.

![Addressable Group Tab][AddressableGroupTab]

## Asset Bundle Builder

The `Asset Bundle Builder` is used to split our mod up into one or more [addressable bundles](https://docs.unity3d.com/Packages/com.unity.addressables@0.8/manual/index.html) and allow us to export them, along with our `catalog` json files.

## Opening the Asset Bundle Builder

The `Asset Bundle Builder` can be opened by going to the `ThunderRoad (SDK)` menu, then clicking `Asset Bundle Builder`. 

![Asset Bundle Builder Menu][AssetBuilderMenu]

It will open a window on your Unity editor.

![Asset Bundle Builder Tab][AssetBuilderTab]







[AssetBuilderMenu]:    {{ site.baseurl }}/assets/getting-started/unity-modding/asset-bundle-builder-menu.jpg
[AssetBuilderTab]:    {{ site.baseurl }}/assets/getting-started/unity-modding/asset-bundle-builder-tab.jpg
[ModFolders]:    {{ site.baseurl }}/assets/getting-started/unity-modding/mod-folders.JPG
[AddressableGroupMenu]:    {{ site.baseurl }}/assets/getting-started/unity-modding/addressable-groups-menu.jpg
[AddressableGroupTab]:    {{ site.baseurl }}/assets/getting-started/unity-modding/addressable-group-tab.jpg

[SDKSetup]:    {{ site.baseurl }}{% link Modding/Getting Started/unity-modding.md %}