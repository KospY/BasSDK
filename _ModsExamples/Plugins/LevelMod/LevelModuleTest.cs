using UnityEngine;
using BS;

namespace BasPluginExample
{
    // This create an level module that can be referenced in the level JSON
    public class LevelModuleTest : LevelModule
    {
        public bool myTestBool;

        public override void OnLevelLoaded(LevelDefinition levelDefinition)
        {
            // Called when the level load
            Debug.Log("OnLevelLoaded " + levelDefinition.gameObject.scene.name);
            initialized = true; // Set it to true when your script are loaded
        }

        public override void Update(LevelDefinition levelDefinition)
        {
            // Called every frame
        }

        public override void OnLevelUnloaded(LevelDefinition levelDefinition)
        {
            // Called when the level unload
            Debug.Log("OnLevelUnloaded " + levelDefinition.gameObject.scene.name);
            initialized = false;
        }

        public override void OnCreatureDeath(LevelDefinition levelDefinition, Creature creature, bool wasPlayer)
        {
            // Called when a creature died
            Debug.Log("OnCreatureDeath " + levelDefinition.gameObject.scene.name + " " + creature.name);
        }
    }
}
