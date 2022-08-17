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

```tip
If you have lots of greyed out groups, you can right click the greyed out group and click "Clear Missing References" 
```


## Creating an Addressable Asset Group

 We will create a new `Addressable Asset Group` and move it into our mods folder structure so we can easily move it to new projects later if needed. This just helps to keep everything about the mod together.

With the `Addressable Group` window open:

1. Click the `New` drop down button.
2. Click `Packed Assets`
3. Right Click the newly created `Packed Assets` group in the `Addressable Group` window.
4. Click `Rename`
5. Name the group `MyMod`

```tip
You can name the Addressable Group anything, like MyMod-Weapon1, MyModAssets, Map1, Map2 etc. They get bundled under our mod folder MyMod using the Asset Bundle Group in a later step. 
```
<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/getting-started/unity-modding/create-addressable-group.mp4" type="video/mp4">
</video>


We have now created our `Addressable Group` for our mod, `MyMod`.

## Organizing Addressable files

We will now move the Addressable files into your mod folder so we can easily back them up, commit them to git, or export them if needed.


1. Your project view should have changed to `Assets/AddressableAssetsData/AssetGroups` and your `MyMod` group should be highlighted.
    - If not navigate to that folder and click your new `MyMod` group.
2. Drag your `MyMod` group into your `Assets/MyMod/Addressable` folder
3. In the project view navigate to `Assets/AddressableAssetsData/AssetGroups/Schemas`
4. Drag your `MyMod_BundledAssetGroupSchema` schema into your `Assets/MyMod/Addressable` folder
5. Drag your `MyMod_ContentUpdateGroupSchema` schema into your `Assets/MyMod/Addressable` folder

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/getting-started/unity-modding/organize-addressable-group.mp4" type="video/mp4">
</video>


You should now have 4 files inside your `Assets/MyMod/Addressable` folder.

1. MyModGroup - Asset Bundle Group
2. MyMod - Addressable Asset Group
3. MyMod_BundledAssetGroupSchema - Bundle Asset Group Schema
4. MyMod_ContentUpdateGroupSchema - Content Update Group Schema


## Adding Addressable Asset Groups to Asset Bundle Group

We need to now add our newly created `Addressable Asset Group` to our `Asset Bundle Group`. This is where if we have multiple Addressable Groups, we would add them here under the mods`Asset Bundle Group`. But in our example we only have one.

1. Navigate to your `Assets/MyMod/Addressables` folder
2. Click the `Asset Bundle Group` called `MyModGroup`
3. In the `Inspector` window, you should see your `Asset Bundle Group`. 
We already added an empty space for our `Addressable Asset Group` in previous steps, but if you haven't click the + icon.
4. Drag and drop our `MyMod` addressable group into the `Element 0` box.

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/getting-started/unity-modding/assetbundle-add-addressable-group.mp4" type="video/mp4">
</video>

We now have everything joined up with our addressable system. 

We can start to add assets into the addressable group `MyMod` so we can export them.





## Adding assets to the Addressable group

An asset needs to be made `addressable` and added to an `Addressable Asset Group` so it can be exported.

### Create a prefab

Lets create a simple cube prefab. Normally you would be creating a weapon or something.

1. Right click in the Unity Hierarchy view and click `3D Object` > `Cube`
2. Drag the newly created `Cube` GameObject into our `Assets/MyMod/Prefabs` folder
3. Click on the `Cube` prefab in the `Prefabs` folder

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/getting-started/unity-modding/create-prefab.mp4" type="video/mp4">
</video>


### Setting the Default Addressable Asset Group

The prefab needs to be made addressable in our `Addressable Asset Group` and be given a **unique** address to be accessed in game.

1. Open the `Addressable Groups` window
2. Right click our `MyMod` group
3. Click `Set as Default`

![AddressableGroupDefault][AddressableGroupDefault]

This will add any new addressable objects to our group by default.


### Making our prefab addressable

Now we can make the prefab addressable.

1. Navigate to `Assets/MyMod/Prefabs`
2. Click the `Cube` prefab.
3. In the `Inspector` window click the `Addressable` checkbox
4. In the box that appears type a unique address for your prefab, like `MyMod.Prefabs.Cube`
5. Press enter so it saves the address.
6. In the `Addressables Groups` window, expand the `MyMod` group
7. In the Labels column, click the empty drop down box.
8. Select `Windows` if its for a PCVR mod, `Android` for Nomad, or select both if the mod will be exported for both games.

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/getting-started/unity-modding/create-addressable-prefab.mp4" type="video/mp4">
</video>

Well done, you have added your asset to the bundle! 

It's nearly ready to be exported! We just need to setup our `Catalog` files.


## Setup Catalogs

The catalog is how we tell B&S how to load our mods.
They are json files which define items, spells, mods, level modules and various effects.

For this mod, we just need a `manifest.json` file to define our mod so B&S will load it.

To create the catalog for our mod, we must go to the `BasSDK-master\BuildStaging\Catalogs` folder. This is not accessible from the `Project` view within Unity. So you need to do it in Windows.

To get their quickly, Right click the `Assets` folder in the Unity `Project` view and click `Show In Explorer`. Then you can go into `BuildStaging\Catalogs`.

Inside the `\BuildStaging\Catalogs` folder, create a folder with the same name as our `MyModGroup` asset bundle has, in this case, a folder called `MyMod`.

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/getting-started/unity-modding/create-catalog-folder.mp4" type="video/mp4">
</video>

### Creating the mod manifest.json

The manifest defines your mod name, description, version and which GameVersion it is for. The GameVersion changes with updates and needs to match the version the game expects, like `0.11.0.0`.

Create a file called `manifest.json` in `BasSDK-master\BuildStaging\Catalogs\MyMod` with the following contents:

```json
{
    "Name": "MyMod",
    "Description": "This is my cool mod",
    "Author": "Wully",
    "ModVersion": "1.0.0",
    "GameVersion": "0.11.0.0"
}
```

If you have having trouble creating it you can download this example here. [U11 Manifest file][U11Manifest] (You might need to right click and click `Save link as` to download it)

If we had any other json files like a Level.json or Item.json files, we could also put them in this `BasSDK-master\BuildStaging\Catalogs\MyMod` folder.

![Catalog Folder][CatalogFolder]


## Exporting the mod

So we now have our `Catalog` with our json files in place, and our addressable groups with our addressable assets setup.

**We can now export our mod!**

We will be using the Asset Bundle Builder to export our mod to the PCVR Steam directory.

## Asset Bundle Builder

The `Asset Bundle Builder` is used to build our mod, we can export individual Asset Bundle Groups containing our addressables, or multiple that may go into the same mod folder.

It will build our mod, and export it, along with our `Catalog` json files in the `BuildStaging\Catalogs\MyMod` folder.

### Opening the Asset Bundle Builder

The `Asset Bundle Builder` can be opened by going to the `ThunderRoad (SDK)` menu, then clicking `Asset Bundle Builder`. 

![Asset Bundle Builder Menu][AssetBuilderMenu]

It will open a window on your Unity editor.

![Asset Bundle Builder Tab][AssetBuilderTab]



### Build the Bundle Group

To build the bundle click the check box on the left side of the bundle name.

Then click `Build asset bundle group(s) selected`

This will build our assets, but not export them yet.

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/getting-started/unity-modding/build-bundle.mp4" type="video/mp4">
</video>

```tip
The first time building may take a long time due to the shaders being compiled. Subsequent builds should be quicker.
```

### Exporting the Bundle Group

Now that we have done a build for the first time it should be quicker in the future.

We can export our mod by **also** clicking the export checkbox on the right side of the bundle group name.

Finally, click `Build asset bundle group(s) selected` to rebuild the mod and export it in one go.


Once it has built, it will copy the `Catalog` json files and the addressable bundles into your `Mod` folder.

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/getting-started/unity-modding/export-mod.mp4" type="video/mp4">
</video>


```warning
Clicking "Export now" will export the assets only, it does package the catalog into a MyMod.jsondb file but it currently goes into the wrong directory. It is not recommended to use the "Export Now" button
```

[AssetBuilderMenu]:    {{ site.baseurl }}/assets/getting-started/unity-modding/asset-bundle-builder-menu.jpg
[AssetBuilderTab]:    {{ site.baseurl }}/assets/getting-started/unity-modding/asset-bundle-builder-tab.jpg
[ModFolders]:    {{ site.baseurl }}/assets/getting-started/unity-modding/mod-folders.JPG
[AddressableGroupMenu]:    {{ site.baseurl }}/assets/getting-started/unity-modding/addressable-groups-menu.jpg
[AddressableGroupTab]:    {{ site.baseurl }}/assets/getting-started/unity-modding/addressable-group-tab.jpg
[AddressableGroupDefault]:    {{ site.baseurl }}/assets/getting-started/unity-modding/group-default.jpg
[CatalogFolder]:    {{ site.baseurl }}/assets/getting-started/unity-modding/catalog-folder.JPG
[SDKSetup]:    {{ site.baseurl }}{% link Modding/Getting Started/unity-modding.md %}

[U11Manifest]:    {{ site.baseurl }}/assets/getting-started/unity-modding/manifest.json