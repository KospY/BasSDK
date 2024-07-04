---
layout: default
has_children: false
parent: Guides
title: Installing Mods
---
{: .warning}
This Wiki is currently under active development, and pages may change drastically. Some pages under this wiki are either out of date or not fully finished.

# How to Install mods
---
# Preffered Installation - In-Game Mod Manager

In-game, go to the "Mods" section in the Main Menu, and install mods through the mod manager. These mods are hosted on [Mod.io](https://mod.io/g/blade-and-sorcery)

Additionally, you can go to [Mod.io](https://mod.io/g/blade-and-sorcery) and subscribe to the mods on there. Then, when you create an account and log in to Mod.io in Blade and Sorcery, your mods will automatically download from your subscribed list.

From the Mod Manager inside Blade and Sorcery, you can disable, uninstall and unsubscribe directly in game.


# Manual Installation
The manual installation for mods is a more advanced way to install mods in to Blade and Sorcery. This is a requirement for manual installations of mod files, and for creating your own mods or testing a mod that is not public. 

For Nexus mods, you can also use [Nexus Vortex](https://www.nexusmods.com/site/mods/1) to download mods and install them. This does not support mods hosted on Mod.io.

## PCVR (Steam)

1. Download the mod from either source

 [Mod.io](https://mod.io/g/blade-and-sorcery){: .btn .btn-blue .mr-4} [Nexus](https://www.nexusmods.com/bladeandsorcery/mods/){: .btn .btn-purple .mr-4}

2. Open your B&S installation folder and go to StreamingAssets. The file path by default is `C:\\Program Files (x86)\Steam\steamapps\common\Blade & Sorcery\BladeAndSorcery_Data\StreamingAssets\Mods`
3. Drag your mod folder from the archive and drop it into `StreamingAssets\Mods`. Be sure to check that your mod folder is in `StreamingAssets\Mods` and not inside some subfolder.

## Nomad (Android)

{: .tip}
Since Android version V63, installing mods manually to Android is now much more difficult. Follow [This guide](https://youtu.be/7H3pfTvzDBc?si=GBgbL1ltAPg3k4lv) to learn how to access the Oculus/Meta Quest 2/3 file system.

1. Download the mod from either source

 [Mod.io](https://mod.io/g/blade-and-sorcery){: .btn .btn-blue .mr-4} [Nexus](https://www.nexusmods.com/bladeandsorcery/mods/){: .btn .btn-purple .mr-4}

2. Extract the mod folder
3. Plug your Quest Headset into your PC
4. Drag and drop mod folder into `This PC\Quest (2/3/Pro)\Internal shared storage\Android\data\com.Warpfrog.BladeAndSorcery\files\Mods`

## Common mod issues with manual installation 

If your mods are not working, here are the solutions to very common issues:

1. Ensure your mods are working for the version of Blade and Sorcery you have installed. Especially common when there is a big update, most mods will be obsolete and unplayable until the modder updates it. Most modders update to the latest version within a few days, but some mods may remain out of date and unusable if abandoned. If installed through the in-game mod manager, mods uploaded for the previous version will be automatically updated (if the modder has updated the mod), otherwise it will be disabled.
2. Make sure your mod folder is not in StreamingAssets\Mods and not inside a subfolder inside StreamingAssets\Mods. As mentioned in the install guide above, if you are seeing random numbers in your folder name you have likely installed your mod incorrectly and accidentally created a subfolder.
    
    ![InstallMods][InstallMods]
    
3. If you still are having issues, please ask for help in `#pcvr-modding-help` or #nomad-modding-help in the Blade and Sorcery [Discord.](https://discord.gg/bladeandsorcery)


[InstallMods]: {{ site.baseurl }}/assets/components/Guides/InstallingMods/InstallMods.png