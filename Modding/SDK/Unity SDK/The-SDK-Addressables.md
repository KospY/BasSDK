# Addressables

## Introduction

Addressables can be quite daunting for some, due to how complex it seems, however once explained it should be more simple to reference and use correctly. 
Addressables are how bundles are extracted now that AssetBundles are obsolete. Addressables are then referenced in JSONs for it to retrieve those prefabs and content from the bundle.

## How do I assign Addressables?

Once you have created your weapon or scene, you must tick the "addressables" box

![](https://i.imgur.com/anhun7k.png)

Additionally, you can move your prefab to the Addressable group (mentioned later in the document). 
Once this is done, it is best you simplify the name of the addressable, and make sure it is unique to avoid any conflicts with other mods. 

## Addressables Groups Window

![](https://i.imgur.com/ID97zih.png)

The "Addressables Groups" Windows is essential for making sure that your addressables are correct. In this window, it lists all of your addressables and the group they are in. All ticked addressables are sent to a "Default" location, which can be set by right clicking the group and setting it as Default. Additionally, you add a group by right clicking the background and selecting "Create New Groups > Packed Assets".

![](https://i.imgur.com/Rmq2GVE.png)

Please note that when extracting, the group you are extracting MUST be set to "Default" to avoid any issues when making your mod.

In the Addressable groups, once the prefabs are inside the groups, you can right click > Simplify name, so that you can create a unique addressable for your mod. A name like "Sword" is not unique enough and can cause potential conflict

![](https://i.imgur.com/r2clOMS.png)

Additionally, you can just completely rename the addressable, and also use the "Addressable Rename Wizard" found here:

![](https://i.imgur.com/dhOCkHJ.png)

In this Wizard, you can rename the addressables easily, which is especially useful for large addressable groups

![](https://i.imgur.com/CtFnoAW.png)

Append : Adds the words to the beginning of the addressables. For example in this image, Example. is added to the beginning, meaning that if the weapon was called "Sword", the addressable would be called "Example.Sword".
Search / Replace : If you put search as "Example" and put "MyWeapon" in to Replace, it will replace all addressables with "Example" to "MyWeapon". This means that if an addressable was "Example.Sword" instead it would be replaced with "MyWeapon.Sword".

## The Mod Builder

![](https://i.imgur.com/0dvZ65j.png)

The Mod Builder extracts your mods! In this, it will list all the Addressable groups, and allow you to extract them in to a specific folder. 
Note: If it is not on the list, press "Refresh available groups"
Note: Remember to set the mod you want to extract as "Default" first.

Here, you can tick what mods/addressables you want to extract. Make sure that you link your Blade and Sorcery directory location also

![](https://i.imgur.com/PeVH6DB.png)

Once you have completed your addressable naming, then must set a "Mod folder name" and click "Build and Export". Here, it will compile shader variants (this may take a while depending on the size of your addressable!). If it asks you to purge all cached data on the addressable, tick yes, save scene if it asks you, and then you have successfully extracted! Your "Mods" folder should contain the folder, along with the bundle inside. 

Note: Do not delete the catelog json or hash, as this allows the game to read these files
Note: You cannot rename the folder name, as these files rely on the naming of the file
Note: Custom shaders will extract with a "unitydefaultshaders" bundle. Setting your addressable to default makes sure this isn't a problem.