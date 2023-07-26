using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThunderRoad.Modules
{
    public class LevelInstancesModule : GameModeModule
    {
        public List<LevelInstance> levelInstances { get; private set; }
        public LevelInstance loadedLevelInstance { get; private set; }
        
        public override IEnumerator OnLoadCoroutine()
        {
            yield return base.OnLoadCoroutine();
            levelInstances = BuildLevelInstances(); //TODO on load it should load the levelInstances from the save file
        }

        public override void OnUnload()
        {
            base.OnUnload();
        }


        public bool TryGetLevelInstance(string guid, out LevelInstance levelInstance)
        {
            levelInstance = null;
            
            if (!Guid.TryParse(guid, out Guid result))
            {
                Debug.LogError($"Unable to get level instance. Unable to parse guid. {guid}");
                return false;
            }
            return TryGetLevelInstance(result, out levelInstance);
        }
        
        public bool TryGetLevelInstance(Guid guid, out LevelInstance levelInstance)
        {
            levelInstance = null;
            foreach (LevelInstance instance in this.levelInstances)
            {
                if (instance.guid == guid)
                {
                    levelInstance = instance;
                    break;
                }
            }
            return levelInstance != null;
        }
        
        public bool LoadLevelInstance(string guid)
        {
            if (!TryGetLevelInstance(guid, out LevelInstance levelInstance))
            {
                Debug.LogError($"Unable to load level instance. Could not get levelInstance for guid: {guid}");
                return false;
            }
            loadedLevelInstance = levelInstance;
            OnLevelInstanceLoaded(levelInstance);
            return true;
        }
        
        public virtual void OnLevelInstanceLoaded(LevelInstance levelInstance)
        {
#if UNITY_EDITOR
            Debug.Log($"LevelInstance Loaded: {levelInstance.levelData.id} - {levelInstance.guid}");
#endif
        }
        
        public bool UnloadLevelInstance(string guid)
        {
            if (loadedLevelInstance == null)
            {
                Debug.LogWarning($"Unable to unload level instance. No instance currently loaded. {guid}");
                return false;
            }

            if (!Guid.TryParse(guid, out Guid result))
            {
                Debug.LogError($"Unable to unload level instance. Unable to parse guid. {guid}");
                return false;
            }

            if (loadedLevelInstance.guid != result)
            {
                Debug.LogError($"Unable to unload level instance. Currently loaded guid [{loadedLevelInstance.guid}] does not match the provided guid: {guid}");
                return false;
            }

            LevelInstance unloadedLevelInstance = loadedLevelInstance;
            OnLevelInstanceUnloaded(unloadedLevelInstance);
            loadedLevelInstance = null;
            return true;
        }

        public virtual void OnLevelInstanceUnloaded(LevelInstance levelInstance)
        {
#if UNITY_EDITOR
            Debug.Log($"LevelInstance Unloaded: {levelInstance.levelData.id} - {levelInstance.guid}");
#endif
        }
        
        public virtual List<LevelInstance> BuildLevelInstances()
        {
            //Default implementation just gets all levels from the catalog which should be shown on the map and creates an instance for each
            this.levelInstances ??= new List<LevelInstance>();
            return this.levelInstances;
        }
    }
}
