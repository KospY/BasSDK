# Unity SDK Setup

This guide will show you how to setup Unity and the BAS SDK.

## Unity Hub Install.

First thing is to download the [Unity Hub](https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.exe) and install it.


## Unity Editor Install

Once installed, you then need to install the current Unity version BAS is using.
Generally having the same MAJOR.MINOR LTS version of Unity is fine. But for full compatability, try to use the same version BAS itself uses.

```tip
U11 uses 2020.3.33f1, but we can use the latest 2020.3 LTS version if necessary.
```

1. Open the Unity Hub.
2. Click Installs on the left side.
3. Click Install Editor.
4. Click Install next to `2020.3` `LTS` version of Unity.
    - If you are building mods for Nomad also check Android Build Support.
5. Click Install.

Unity will now download and install the editor.

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/getting-started/unity-modding/editor-install.mp4" type="video/mp4">
</video>


## BAS SDK Unity Project

The BAS SDK is a Unity project which we will download and open in Unity.

It has all the tools to build mods and export them.

1. Go to the BASSDK Git repository [here](https://github.com/KospY/BasSDK/)
2. Ensure the `master` branch is selected in the branch drop down.
3. Click the `Code` drop down button
4. Click download zip.
5. Once the SDK is download, extract it using zip, winrar, or 7zip to somewhere preferably on a fast SSD.

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/getting-started/unity-modding/sdk-install.mp4" type="video/mp4">
</video>

## Opening the SDK Project

We will use the Unity Hub to open the BAS SDK.

1. Open the Unity Hub.
2. Click `Projects` on the left side
3. Click `Open` at the top of the projects section.
4. Navigate to where you extracted the `BasSDK-master` folder.
5. Click the folder `BasSDK-master`
6. Click `Open`.

Unity will then open the project and start to process and import the project files. This may take some time, so let it complete.

Once the import is finished, you should see the Unity editor scene and/or game view.

<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/getting-started/unity-modding/import-sdk.mp4" type="video/mp4">
</video>

