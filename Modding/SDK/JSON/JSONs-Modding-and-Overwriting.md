# JSONs: Modding and Overwriting

To create a mod, you must first create a folder inside the "Mods" folder stored in StreamingAssets (steamapps\common\Blade & Sorcery\BladeAndSorcery_Data\StreamingAssets\Mods). Name this mod something unique, then obtain a [manifest.json](https://github.com/KospY/BasSDK/blob/master/_ModsExamples/ModFolder/WitchBroom/manifest.json) from either this hyperlink or from another mod.
A manifest.json file is required for Blade and Sorcery to detect the mod you have created, otherwise the folder/mod will simply be ignored. In this file, you must edit the name of the mod, the mod version, the description and the author. 

As the game updates, the GameVersion should also be changed if the mod is created for it. For example, the GameVersion must be "9.0" if the mod is updated for Update 9.0, as mod loader programs like Vortex use this information to tell the users if the mod if out of date. You can also copy the manifest.json from other mods, just make sure that you edit the contents inside.

The game will only read folders containing a manifest.json, and the Nexus Vortex mod manager will also use this manifest to correctly detect your mod, including reading the name and version, as well as detecting if the mod is updated for the correct version.

Once the manifest is done, just copy/paste the JSON file you want to mod from the default\bas folder to your custom directory. Each JSON found in the mod folder will override the default one using it's ID.

It's highly recommended to keep in the customized JSON only the fields you want to change, this way, if another mods do changes to another field in the same JSON it will not conflict. 

Each customized JSON should contain at least the fields `$type`, `id `and `version`. It is recommended that you do not edit these, and that if there is a new update that changes the JSON files you edit, it is recommended to start with a fresh new json parameters to prevent any issues.

You will find below an example of a clean mod to increase the health and locomotion speed of the player character.

File "Creature_PlayerDefaultMale.json"
```
{
  "$type": "BS.CreatureData, Assembly-CSharp",
  "id": "PlayerDefaultMale",
  "version": 5,
  "health": 1000,
  "locomotionSpeed": 5.0
}
```

Please note that if you edit a file such as a creature json, that if you do not change any other parameter, it is recommended to delete those parameters from your modded version to prevent mod conflict. An overwritten file will only overwrite the components inside the file itself, so for the example above, other fields not included inside that edit will not change, such as focus and manaRegen, and will use the Default folder's parameters from this instead (unless you have another mod which changes these values) 