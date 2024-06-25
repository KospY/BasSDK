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

{: .important}
Blade and Sorcery uses 2021.3.38f1.

1. Open the Unity Hub.
2. Click Installs on the left side.
3. Click Install Editor.
4. Unity Version 2021.3.38 is a previous Unity LTS version, so you must go to "Archive" then "download archive".
Alternatively, you can click [Here.](unityhub://2021.3.38f1/7a2fa5d8d101)  
5. Click on 2021 at the top of the page, and locate `2021.3.38f1`. 
6. Click Install, allow the Unity page to access programs outside of the webpage (for Unity Hub) and agree to the Terms of Service.

Unity will now download and install the editor.

<video src="/assets/components/Guides/UnitySDKSetup/editor-install.mp4" width="880" height="440" controls></video>

## Sync SDK repository on your computer

---

The SDK is hosted on Github. To make further updates easily, we highly recommend to install a version control software like Github Desktop, that will allow you to update the repository and be aware of any local changes.

1. Go and install GitHub Desktop: [https://desktop.github.com/](https://desktop.github.com/)
2. Go to the [SDK Git repository](https://github.com/KospY/BasSDK)
3. Copy the Git URL
    
  <video src="/assets/components/Guides/UnitySDKSetup/CopyURL.mp4" width="880" height="440" controls></video>
    
4. Open Github Desktop, go to `File> Clone repository` and paste the URL to clone the repository on your machine
    
      <video src="/assets/components/Guides/UnitySDKSetup/GitClone.mp4" width="880" height="440" controls></video>
    
{: .note}
Once synced, you can switch from different SDK version by changing branch (`Master` will always be the latest version of the SDK)
![Fetch][Fetch]

{: .important}
Fetch button will update your local repository if any updates has been pushed on the online repositor

![Branch][Branch]

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

<video src="/assets/components/Guides/UnitySDKSetup/nomad-log-windows.mp4" width="880" height="440" controls></video>

[EditorInstall]: {{ site.baseurl }}/assets/components/Guides/UnitySDKSetup/editor-install.mp4
[CopyURL]: {{ site.baseurl }}/assets/components/Guides/UnitySDKSetup/CopyURL.mp4
[GitClone]: {{ site.baseurl }}/assets/components/Guides/UnitySDKSetup/GitClone.mp4
[Fetch]: {{ site.baseurl }}/assets/components/Guides/UnitySDKSetup/Fetch.png
[Branch]: {{ site.baseurl }}/assets/components/Guides/UnitySDKSetup/Branch.png