---
parent: ThunderScript
grand_parent: Guides
---
# ThunderScript

## What is ThunderScript

ThunderScript is a base class for any “global” scripts that a modder wants to introduce in their mods; ThunderScript mods get loaded without needing any additional JSON. They replace LevelModules that are added to Level_Master like in U11 and previous versions. They provide many similar methods to override as would be found in a LevelModule. You can have as many ThunderScript classes in your mod as you want or need!

## When are ThunderScripts Loaded.

ThunderScripts are loaded when the mod itself loads, only after the “Play” button is pressed in the main menu screen. ThunderScripts can’t load before then, meaning you can’t modify the main menu without the use of third party tools. If one or multiple ThunderScript classes are present in your DLL, the mod loading process will pick them up and run their methods. They’re loaded only once currently (as of 12.2), but in the future it’s possible that they will be able to load and unload.

## How do I use ThunderScript

To use ThunderScript, inherit from the ThunderScript class like you would with ThunderBehaviour, a LevelModule, or ItemModule. From there, you can override its base methods to add functionality to your script. The example script below can be used as a helpful guide to the basics of a ThunderScript. Keep in mind that you only need to override the methods you need, but for the purposes of demonstration, this script overrides all of the methods which are unique to ThunderScript:

```csharp
using ThunderRoad;
using UnityEngine;
// You will likely need other using imports, such as using System, or using System.Collections.Generic

namespace ExampleThunderScript
{
    // This example script will keep track of the number of creatures the player has killed, and send that number to the debug console log
    public class ExampleScript : ThunderScript
    {
				// Adding in a public static reference here turns this script into a singleton, and allows you to access this ThunderScript from other classes using ExampleScript.instance
        public static ExampleScript instance;

        public bool printActive;
        public int playerKills;

        // You can utilize ModOptions with ThunderScripts to add configurable settings to your ThunderScript code
        [ModOption]
        public static float printFrequency;

        protected float lastPrintTime = 0f;

        // Runs when the script is loaded
        // This runs only once, and is generally a good place for you to add handlers to STATIC events like EventManager.onLevelLoad or EventManager.onCreatureSpawn
        public override void ScriptLoaded(ModManager.ModData modData)
        {
            base.ScriptLoaded(modData);
            // This is a good place to add some kind of "start up" message so you can tell for certain that your script has loaded
            Debug.Log($"Loaded my example script! It is a script for {modData.Name}, and its containing DLL is in {modData.fullPath}");
            // This method is also a great place to set this as the current singleton instance, if you set this script up as a singleton
            instance = this;
            playerKills = 0;
        }

        private void CreatureKillHandler(Creature creature, Player player, CollisionInstance collisionInstance, EventTime eventTime)
        {
            // This event fires twice, once on the "kill start" (before the creature is actually considered "dead") and once on the "kill end" (after all the code is finished, and the creature is official dead)
            if (eventTime == EventTime.OnStart) return;
            // If the player creature is null, we know that this is the player that just died. We don't want to increment the kills count
            if (Player.currentCreature == null) return;
            // If this creature wasn't an enemy of the player, choose not to increment the kills count
            if (!creature.IsEnemy(Player.currentCreature)) return;
            playerKills++;
        }

        // Runs when the script is enabled
        // In 12.1, this only runs once after the script is loaded
        // This is also a fine place to add handlers to static events, perhaps even recommended if in the future a mod can be loaded but start disabled
        public override void ScriptEnable()
        {
            base.ScriptEnable();
            EventManager.onCreatureKill += CreatureKillHandler;
            printActive = true;
        }

        // Runs every physics frame, meaning it's not necessarily running as much as Update
        // This is a good place to add anything which adds force, modifies velocities, etc.
        // Generally, you want to use Time.fixedDeltaTime in this method to get the time difference between fixed updates
        public override void ScriptFixedUpdate()
        {
            base.ScriptFixedUpdate();
            // Since there's nothing to do here in this script, this method is left empty
            // If you were making a script that made the player fly, then this would be where you'd want to add the force
        }

        // Every frame prior to animation, IK, and rendering
        // This is not synchronized with physics; multiple Updates can happen between every Fixed Update, or vice-versa
        // You generally will want to use Time.deltaTime here to get the time difference between updates
        public override void ScriptUpdate()
        {
            base.ScriptUpdate();
            // Since our debug print message isn't physics related, and doesn't need to happen after the animation and IK run, we can perform the logic here
            if (printActive && Time.time >= lastPrintTime + printFrequency)
            {
                Debug.Log($"{Time.time}: The player has killed {playerKills} enemies!\nThis log will print again in {printFrequency} seconds.");
                lastPrintTime = Time.time;
            }
        }

        // Every frame after IK and animation have finished
        // Some of the IK used in B&S also runs in LateUpdate, so if you find odd behaviour where things aren't aligned properly with creatures, you may need to do more work to get things aligned right
        public override void ScriptLateUpdate()
        {
            base.ScriptLateUpdate();
            // Since there's nothing to do here in this script, this method is left empty
            // If you were doing something regarding any creature's hand positions, this would be the place to do it
        }

        // Runs once when the script is disabled
        // In 12.1, this never happens, but we hope to get scripts set up to disable and unload in the future; it's a good idea to get into the habit of making this work now!
        public override void ScriptDisable()
        {
            base.ScriptDisable();
            // Always be sure to unsubscribe from events once you *know* your script is done with them. This helps prevent garbage from collecting
            EventManager.onCreatureKill -= CreatureKillHandler;
            printActive = false;
        }

        // Runs once when the script is unloaded
        // As noted with ScriptDisable() this doesn't get executed at all, but you should get into the habit of using this should the functionality be introduced
        // When it's functional, this is the final endpoint for your mod! If your mod gets re-activated, it'll go through ScriptLoaded again
        public override void ScriptUnload()
        {
            base.ScriptUnload();
            // Unset the instance if this is it
            if (instance == this) instance = null;
        }
    }
}
```