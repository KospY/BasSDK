---
parent: SDK-HowTo
grand_parent: Guides
---
# How to install mods

# Preferred Installation

{: .important}
In-game, go to the "Mods" section in the Main Menu, and install mods through the mod manager. These mods are hosted on [Mod.io](https://mod.io/g/blade-and-sorcery)

### Manual Installation (For advanced users or Nexus Downloads)

## PCVR (Steam)

1. Download the mod from either source

 [Mod.io](https://mod.io/g/blade-and-sorcery){: .btn .btn-blue .mr-4} [Nexus](https://www.nexusmods.com/bladeandsorcery/mods/){: .btn .btn-purple .mr-4}

2. Open your B&S installation folder and go to StreamingAssets. The file path by default is `C:\\Program Files (x86)\Steam\steamapps\common\Blade & Sorcery\BladeAndSorcery_Data\StreamingAssets\Mods`
3. Drag your mod folder from the archive and drop it into `StreamingAssets\Mods`. Be sure to check that your mod folder is in `StreamingAssets\Mods` and not inside some subfolder.

## Nomad (Android)

1. Download the mod from either source

 [Mod.io](https://mod.io/g/blade-and-sorcery){: .btn .btn-blue .mr-4} [Nexus](https://www.nexusmods.com/bladeandsorcery/mods/){: .btn .btn-purple .mr-4}

2. Extract the mod folder
3. Plug your Quest Headset into your PC
4. Drag and drop mod folder into `This PC\Quest (2/3/Pro)\Internal shared storage\Android\data\com.Warpfrog.BladeAndSorcery\files\Mods`


### Common mod issues with manual installation

If your mods are not working, here are the solutions to very common issues:

1. Ensure your mods are working for the version of Blade and Sorcery you have installed. Especially common when there is a big update, most mods will be obsolete and unplayable until the modder updates it. Most modders update to the latest version within a few days, but some mods may remain out of date and unusable if abandoned. If installed through the in-game mod manager, mods uploaded for the previous version will be automatically updated (if the modder has updated the mod), otherwise it will be disabled.
2. Make sure your mod folder is not in StreamingAssets\Mods and not inside a subfolder inside StreamingAssets\Mods. As mentioned in the install guide above, if you are seeing random numbers in your folder name you have likely installed your mod incorrectly and accidentally created a subfolder.
    
    ![InstallMods][InstallMods]
    
3. If you still are having issues, please ask for help in `#pcvr-modding-help` or #nomad-modding-help in the Blade and Sorcery Discord.
[Discord](https://discord.gg/bladeandsorcery){: .btn .btn-blue }

[InstallMods]: {{ site.baseurl }}/assets/components/Guides/InstallingMods/InstallMods.png