namespace ThunderRoad
{
    using System.Collections.Generic;
    using UnityEngine;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#else
    using EasyButtons;
#endif

    public abstract class AreaCollectionData : CatalogData, IAreaBlueprintGenerator
    {
        public string GetId()
        {
            return id;
        }

        public abstract List<AreaData.AreaConnection> GetConnections();
        public abstract AreaData.AreaConnection GetConnection(int index);

        public abstract bool IsSpawnable();

        public abstract List<AreaRotationHelper.Rotation> GetAllowedRotation();

        public virtual SpawnableArea GetSpawnableArea(AreaRotationHelper.Rotation rotation,
                                                        int entranceIndex,
                                                        Vector3 entrancePosition,
                                                        int numberCreature,
                                                        bool isShareNPCAlert
                                                        )
        {
            IAreaBlueprintGenerator.SpawnableBlueprint spawnableBp = GetSpawnableBluePrint(rotation, entranceIndex, entrancePosition);
            SetCreatureDataToSpawnableArea(spawnableBp, numberCreature, isShareNPCAlert);
            return spawnableBp.GetRoot();
        }


        public abstract IAreaBlueprintGenerator.SpawnableBlueprint GetSpawnableBluePrint(AreaRotationHelper.Rotation rotation,
                                                                                int entranceIndex,
                                                                                Vector3 entrancePosition);

        public abstract void SetCreatureDataToSpawnableArea(IAreaBlueprintGenerator.SpawnableBlueprint spawnableBp,
                                                    int numberCreature,
                                                    bool isShareNPCAlert);

#if UNITY_EDITOR
        [Button]
        public void PreviewAreaAndDisableCondition()
        {
            PreviewAreaWithItemAndDisableCondition(false);
        }

        [Button]
        public void PreviewAreaWithItemAndDisableCondition()
        {
            PreviewAreaWithItemAndDisableCondition(true);
        }

        public virtual void PreviewAreaWithItemAndDisableCondition(bool spawnItem = false)
        {
            PreviewArea(spawnItem);
            DisableOnCondition[] disableOnCondition = Object.FindObjectsOfType<DisableOnCondition>();
            for (int i = 0; i < disableOnCondition.Length; i++)
            {
                disableOnCondition[i].gameObject.SetActive(false);
            }
        }
        
        public virtual void PreviewArea(bool spawnItem = false)
        {
            UnityEditor.SceneAsset areaSceneAsset = Catalog.EditorLoad<UnityEditor.SceneAsset>(ThunderRoadSettings.current.areaSceneAddress);

            if (areaSceneAsset == null)
            {
                Debug.LogError("Area scene at address : " + ThunderRoadSettings.current.areaSceneAddress + " not found");
                return;
            }

            string areaScenePath = UnityEditor.AssetDatabase.GetAssetPath(areaSceneAsset);
            UnityEditor.SceneManagement.EditorSceneManager.CloseScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), false);
            UnityEngine.SceneManagement.Scene dungeonScene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(areaScenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);

            if (dungeonScene == null)
            {
                Debug.LogError("Scene : " + areaScenePath + " not found");
                return;
            }

            AreaManager areaManager = UnityEngine.Object.FindObjectOfType<AreaManager>();
            if (areaManager == null)
            {
                GameObject areaManagerRoot = UnityEngine.Object.Instantiate(new GameObject());
                areaManagerRoot.name = "PreviewArea";
                areaManager = areaManagerRoot.AddComponent<AreaManager>();
            }

            IAreaBlueprintGenerator.SpawnableBlueprint bluePrint = GetSpawnableBluePrint(AreaRotationHelper.Rotation.Front, -1, Vector3.zero);
            areaManager.EditorSpawn(bluePrint.GetRoot(), spawnItem);
        }

#endif //UNITY_EDITOR
    }
}