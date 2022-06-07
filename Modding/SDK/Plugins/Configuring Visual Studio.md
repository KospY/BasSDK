# Configuring Visual Studio

## Introduction

Plugins are DLL files containing custom C# code for the game. It allows modders to do custom logic and advanced stuff.

To create a plugin, you need to download and use Visual Studio and have some notions of C# programming and Unity.

## Configuring Visual Studio

First create a new **class library (.net framework)** project in Visual studio.

![image](https://i.imgur.com/Q4stb6P.png)

Once done, you need to reference the Unity and game dll.

![image](https://i.imgur.com/E9zY53f.png)

What you need here is relative to the thing you want to do, but a good start will be adding these references:
* **UnityEngine.dll** - Unity engine
* **UnityEngine.CoreModule.dll** - Unity core module
* **UnityEngine.PhysicsModule.dll** - Unity physic module
* **Assembly-CSharp.dll** - Game code

![image](https://i.imgur.com/PnwtjE3.png)

After that, you should be ready to code.
To get started, please read the documentation about Item/Level modules and take a look at some examples like the [Witch Broom](https://github.com/KospY/BasSDK/tree/master/Plugins/WitchBroom) or [Shock blade](https://github.com/KospY/BasSDK/tree/master/Plugins/ShockBlade)
