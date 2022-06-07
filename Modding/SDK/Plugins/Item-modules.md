# Item Modules

## Introduction

Item modules are custom classes that can be loaded on each item. It's pretty useful to add different behaviors to an item without conflicting with others mods, as well as providing some parameters that can be directly edited from the JSON file. 

Any module added to a JSON is added to the item, so you can add as many modules as you want on any item (default or  even modded ones).

## C# code

Item modules are done in C# using visual studio, to create a new module, you need to create a new class that derives from `ItemModule`

```
public class ItemModuleTest : ItemModule
{
    public override void OnItemLoaded(Item item)
    {
        base.OnItemLoaded(item);
        // Called when the Item is pooled or spawned
    }

    public override void OnItemDataRefresh()
    {
        // Called when the JSON catalog are reloaded
    }
}
```

Item module is not a component (monobehaviour), therefore it can be used as a container for data only (so others scripts could query it to get custom information). However, if you need extra functionalities, it is best to add a custom component yourself from the load method. 

You will find below an example from the Shock Blade.

```
using BS;
namespace BasPluginExample
{
    public class ItemModuleShock : ItemModule
    {
        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<ItemShock>();
        }
    }
}
```

And the corresponding MonoBehaviour added to the item object:

```
using UnityEngine;
using BS;

namespace BasPluginExample
{
    // The item module will add a unity component to the item object.
    // This component will apply a shock effect on the item when trigger is pressed
    public class ItemShock : MonoBehaviour
    {
        protected EffectShock effectShock;

        protected void Awake()
        {
            this.GetComponent<Item>().OnHeldActionEvent += OnHeldAction;
        }

        protected void Start()
        {
            foreach (EffectData effectData in this.GetComponent<Item>().effects)
            {
                if (effectData is EffectShock)
                {
                    effectShock = effectData as EffectShock;
                    break;
                }
            }
        }

        public void OnHeldAction(Interactor interactor, Handle handle, Interactable.Action action)
        {
            if (action == Interactable.Action.AlternateUseStart)
            {
                if (effectShock != null)
                {
                    if (effectShock.isActive) effectShock.Stop();
                    else effectShock.Play(1, 9999999, 5);
                }
            }
            if (action == Interactable.Action.Ungrab)
            {
                if (effectShock != null) effectShock.Stop();
            }
        }
    }
}
```

For more information about the different available methods of MonoBehaviour, see  https://docs.unity3d.com/ScriptReference/MonoBehaviour.html

## JSON reference

Once you're done with your C# code, just compile and copy the resulting DLL to the mod folder in SteamingAssets.

All the DLL get loaded by the game on start, so the next step is to reference the module class in a JSON.

To add the module to an item, just look at the `modules` array in the item JSON and add a new module with this syntax: 
 `"$type": "YourNamespace.YourItemModuleName, YourAssemblyName"`

Here is an example below from the thrust module of the WitchBroom:

```
...
"modules": [
	{
          "$type": "BasPluginExample.ItemModuleThrust, WitchBroom",
          "minForce": 600,
	  "maxForce": 1300
	}
],
...
```
Modules are fully deserialized so don't hesitate to declare some public variables (like in the example above with `minForce` and `maxForce`) to enhance the modularity of your plugins. 
