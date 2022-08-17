# Unity Modding

We are going to create a mod called `MyMod`. It will have a single prefab which will get packed into an addressable bundle.

You will need Unity installed and the BAS SDK downloaded. You can follow the [Unity and SDK Setup Guide][SDKSetup] to do so.

### Asset Bundle Builder

The `Asset Bundle Builder` is used to split our mod up into one or more [addressable bundles](https://docs.unity3d.com/Packages/com.unity.addressables@0.8/manual/index.html) and allow us to export them, along with our `catalog` json files.

The `Asset Bundle Builder` can be opened by going to the `ThunderRoad (SDK)` menu, then clicking `Asset Bundle Builder`. 

![Asset Bundle Builder Menu][AssetBuilderMenu]

It will open a window on your Unity editor.

![Asset Bundle Builder Tab][AssetBuilderTab]

[AssetBuilderMenu]:    {{ site.baseurl }}/assets/getting-started/unity-modding/asset-bundle-builder-menu.jpg
[AssetBuilderTab]:    {{ site.baseurl }}/assets/getting-started/unity-modding/asset-bundle-builder-tab.jpg
[SDKSetup]:    {{ site.baseurl }}{% link Modding/Getting Started/unity-modding.md %}