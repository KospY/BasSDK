# Level Modules

## Introduction

Level modules are custom classes that can be loaded on each level. It allows different behaviors to run on each level without conflicting with others mods, as well as providing some level parameters that can be directly edited from the JSON file. You can add as many modules as you want on any existing or custom level.

Any module added to a level JSON is called when the level load and unload, with the exception of the "Master" level.
The master level is a specific scene that is always loaded and never get unloaded. So if you need a general / game script to run the best way is to add the module to the master level.

## C# code

Level module are done in C# using visual studio, to create a new module, you need to create a new class that derives from `LevelModule`

```
public class LevelModuleTest : LevelModule
{
    public override void OnLevelLoaded(LevelDefinition levelDefinition)
    {
        // Called when the level load
        initialized = true; // Set it to true when your script are loaded
    }

    public override void Update(LevelDefinition levelDefinition)
    {
        // Called every frame
    }

    public override void OnLevelUnloaded(LevelDefinition levelDefinition)
    {
        // Called when the level unload
        initialized = false;
    }

    public override void OnCreatureDeath(LevelDefinition levelDefinition, Creature creature, bool wasPlayer)
    {
        // Called when a creature died
    }
}
```

Level modules are not a component (monobehaviour), and it can be used only as a container for data if needed (so others scripts could query it to get customized information).

## JSON reference

Once you're done with your C# code, just compile and copy the resulting DLL to the mod folder in SteamingAssets.

All the DLL get loaded by the game on start, so the next step is to reference the module class in a JSON.

To add the module to a level, just look at the `modules` array in the `modes` array in the level JSON and add a new module with this syntax: 

`"$type": "YourNamespace.YourItemModuleName, YourAssemblyName"`

The game already uses modules internally, here is an example of the Home level JSON:

```
{
   "$type":"BS.LevelData, Assembly-CSharp",
   "id":"Home",
   "version":2,
   "modes":[
      {
         "name":"Default",
         "modules":[
            {
               "$type":"BS.LevelModuleDeath, Assembly-CSharp",
               "behaviour":1,
               "deathWaitTime":10.0,
               "deathSlowMo":0.05,
               "deathSlowMoDuration":0.25
            },
            {
               "$type":"BS.LevelModuleCleaner, Assembly-CSharp",
               "cleanerRate":5.0
            }
         ]
      }
   ]
}
```
Modules are fully deserialized so don't hesitate to declare some public variables to enhance the modularity of your plugins. 

To add a module to an existing level, create a new JSON with the minimal fields (`$type`, `id`, and `version`) and add your corresponding module to the `modules` array. 

For example, if you want to add the save functionality to the arena (saving player equipped weapon when leaving), you just need to put this in the JSON:

```
{
   "$type":"BS.LevelData, Assembly-CSharp",
   "id":"Arena",
   "version":2,
   "modes":[
      {
         "name":"Default",
         "modules":[
            {
               "$type":"BS.LevelModuleSave, Assembly-CSharp"
            }
         ]
      }
   ]
}
```

