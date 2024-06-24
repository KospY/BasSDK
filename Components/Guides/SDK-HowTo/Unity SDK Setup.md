---
parent: SDK-HowTo
grand_parent: Guides
---
# Unity SDK Setup

The SDK is based on Unity, this guide will show you how to setup Unity and the SDK.

## Unity Editor install

---

First thing is to download the [Unity Hub](https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.exe) and install it.

Once installed, you then need to install the current Unity version BAS is using.
Generally having the same MAJOR.MINOR LTS version of Unity is fine. But for full compatibility, try to use the same version BAS itself uses.

<aside>
🗒️ U12.1 use 2021.3.17f1, but we can use the latest 2021.3 LTS version if necessary.

</aside>

1. Open the Unity Hub.
2. Click Installs on the left side.
3. Click Install Editor.
4. Click Install next to `2021.3` `LTS` version of Unity.
    - If you are building mods for Nomad also check Android Build Support.
5. Click Install.

Unity will now download and install the editor.

[This video may use the wrong version, but the method stay the same](Unity%20SDK%20Setup%204f14e579b2724ea9a38053af1158989b/editor-install.mp4)

This video may use the wrong version, but the method stay the same

## Sync SDK repository on your computer

---

The SDK is hosted on Github. To make further updates easily, we highly recommend to install a version control software like Github Desktop, that will allow you to update the repository and be aware of any local changes.

1. Go and install GitHub Desktop: [https://desktop.github.com/](https://desktop.github.com/)
2. Go to the [SDK Git repository](https://github.com/KospY/BasSDK)
3. Copy the Git URL
    
    [firefox_OxTdSUw6j3.mp4](Unity%20SDK%20Setup%204f14e579b2724ea9a38053af1158989b/firefox_OxTdSUw6j3.mp4)
    
4. Open Github Desktop, go to `File> Clone repository` and paste the URL to clone the repository on your machine
    
    [GitHubDesktop_qrB3XsbQRN.mp4](Unity%20SDK%20Setup%204f14e579b2724ea9a38053af1158989b/GitHubDesktop_qrB3XsbQRN.mp4)
    

<aside>
🗒️ Once synced, you can switch from different SDK version by changing branch (`Master` will always be the latest version of the SDK

![Untitled](Unity%20SDK%20Setup%204f14e579b2724ea9a38053af1158989b/Untitled.png)

</aside>

<aside>
🗒️ Fetch button will update your local repository if any updates has been pushed on the online repositor

![Untitled](Unity%20SDK%20Setup%204f14e579b2724ea9a38053af1158989b/Untitled%201.png)

</aside>

## Open SDK with Unity

---

We will use the Unity Hub to open the BAS SDK.

1. Open the Unity Hub.
2. Click `Projects` on the left side
3. Click `Open` at the top of the projects section.
4. Navigate to where you extracted the `BasSDK-master` folder.
5. Click the folder `BasSDK-master`
6. Click `Open`.

Unity will then open the project and start to process and import the project files. This may take some time, so let it complete.

Once the import is finished, you should see the Unity editor scene and/or game view.

[import-sdk.mp4](Unity%20SDK%20Setup%204f14e579b2724ea9a38053af1158989b/import-sdk.mp4)