using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Text;
using UnityEngine.AI;

#if DUNGEN
using DunGen;
using DunGen.Adapters;
using DunGen.Graph;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class Level : MonoBehaviour
    {
        public static Level current;
        public static Level master;
        public Transform playerStart;
        public List<CustomReference> customReferences;

        [NonSerialized]
        public State state = State.None;

        [NonSerialized]
        public bool loaded;

#if DUNGEN
        [Header("Dungen")]
        public string dungeonFlowAddress;
        public OcclusionCulling occlusionCulling = OcclusionCulling.Default;
        public bool staticBatchRooms = true;
#endif

        public enum OcclusionCulling
        {
            Default,
            Dungen,
        }

        [Serializable]
        public class CustomReference
        {
            public string name;
            public List<Transform> transforms;
        }

        public enum State
        {
            None,
            Failure,
            Success,
        }

#if DUNGEN
        [NonSerialized]
        public RuntimeDungeon dungeonGenerator;
        [NonSerialized]
        public UnityNavMeshAdapter dungeonNavMeshAdapter;
        [NonSerialized]
        public AdjacentRoomCulling adjacentRoomCulling;


        [Button]
        public void SetCustomOcclusionCulling(OcclusionCulling occlusionCulling)
        {
            if (adjacentRoomCulling)
            {
                if (occlusionCulling == OcclusionCulling.Dungen)
                {
                    adjacentRoomCulling.enabled = true;
                }
                else if (occlusionCulling == OcclusionCulling.Default)
                {
                    adjacentRoomCulling.enabled = false;
                }
                this.occlusionCulling = occlusionCulling;
            }
            this.occlusionCulling = OcclusionCulling.Default;
        }
#endif

        [Button]
        public static void CheckLightMapMode()
        {
            Debug.Log("Lightmap mode: " + LightmapSettings.lightmapsMode);
        }

        [Button]
        public static void ChangeLightMapMode(LightmapsMode lightmapsMode)
        {
            LightmapSettings.lightmapsMode = lightmapsMode;
            Debug.Log("Lightmap mode set to: " + LightmapSettings.lightmapsMode);
        }
   
        [Button]
        public static void TetrahedralizeLightProbes()
        {
            // Fix light probes being wrong!!!
            LightProbes.Tetrahedralize();
        }

        private void OnValidate()
        {
#if DUNGEN
            if (occlusionCulling == OcclusionCulling.Default)
            {
                if (adjacentRoomCulling && adjacentRoomCulling.enabled) adjacentRoomCulling.enabled = false;
            }
            else if (occlusionCulling == OcclusionCulling.Dungen)
            {
                if (adjacentRoomCulling && !adjacentRoomCulling.enabled) adjacentRoomCulling.enabled = true;
            }
            dungeonGenerator = GameObject.FindObjectOfType<RuntimeDungeon>();
            if (dungeonGenerator)
            {
                dungeonGenerator.Generator.DungeonFlow = null;
                dungeonGenerator.GenerateOnStart = false;
            }
#endif
        }

        protected virtual void Awake()
        {
            if (gameObject.scene.name.ToLower() == "master")
            {
                master = this;
                return;
            }
            else
            {
                current = this;
            }

            if (!playerStart)
            {
                PlayerSpawner playerSpawner = PlayerSpawner.GetLevelStart();
                if (playerSpawner)
                {
                    playerStart = playerSpawner.transform;
                }
            }
#if DUNGEN
            dungeonGenerator = GameObject.FindObjectOfType<RuntimeDungeon>();
            adjacentRoomCulling = GameObject.FindObjectOfType<AdjacentRoomCulling>();
            dungeonNavMeshAdapter = GameObject.FindObjectOfType<UnityNavMeshAdapter>();

            if (dungeonNavMeshAdapter)
            {
                dungeonNavMeshAdapter.enabled = false;
            }

            if (dungeonGenerator)
            {
                dungeonGenerator.Generator.OnGenerationStatusChanged += OnGenerationStatusChanged;
                if (!Level.master)
                {
                    // If master scene not loaded
                    Addressables.LoadAssetAsync<DungeonFlow>(dungeonFlowAddress).Completed += (handle) =>
                    {
                        if (handle.Status == AsyncOperationStatus.Succeeded)
                        {
                            dungeonGenerator.Generator.DungeonFlow = handle.Result;
                            GenerateDungeon();
                        }
                        else
                        {
                            Debug.LogError("Could not find dungeon flow asset at address: " + dungeonFlowAddress);
                        }
                    };
                }
            }

            if (adjacentRoomCulling)
            {
                adjacentRoomCulling.enabled = (dungeonGenerator && occlusionCulling == OcclusionCulling.Dungen);
            }
#endif
        }
#if DUNGEN
        [Button]
        public void GenerateDungeon()
        {
            if (dungeonGenerator && Application.isPlaying)
            {
                dungeonGenerator.Generate();
            }
        }

        [Button]
        public void GenerateNavMesh()
        {
            if (dungeonNavMeshAdapter && Application.isPlaying)
            {
                dungeonNavMeshAdapter.BakeMode = UnityNavMeshAdapter.RuntimeNavMeshBakeMode.PreBakedOnly;
                dungeonNavMeshAdapter.enabled = true;
                dungeonNavMeshAdapter.Generate(dungeonGenerator.Generator.CurrentDungeon);
            }
        }

        [Button]
        public void GenerateGlobalNavMesh()
        {
            NavMeshSurface surface = this.gameObject.GetComponent<NavMeshSurface>();
            if (!surface) surface = this.gameObject.AddComponent<NavMeshSurface>();
            surface.BuildNavMesh();
        }

        private void OnGenerationStatusChanged(DungeonGenerator generator, GenerationStatus status)
        {
            if (status != GenerationStatus.Complete) return;

            SceneManager.MoveGameObjectToScene(generator.CurrentDungeon.gameObject, this.gameObject.scene);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Seed: " + generator.ChosenSeed);
            stringBuilder.AppendLine("Generation time: " + generator.GenerationStats.TotalTime + " ms");
            stringBuilder.AppendLine("Main room count: " + generator.GenerationStats.MainPathRoomCount);
            stringBuilder.AppendLine("Branch room count: " + generator.GenerationStats.BranchPathRoomCount);
            stringBuilder.AppendLine("Total room count: " + generator.GenerationStats.TotalRoomCount);
            stringBuilder.AppendLine("Retry count: " + generator.GenerationStats.TotalRetries);
            Debug.Log(stringBuilder.ToString());

            GenerateNavMesh();

            PlayerSpawner playerSpawner = PlayerSpawner.GetLevelStart();
            if (playerSpawner)
            {
                playerStart = playerSpawner.transform;
                if (adjacentRoomCulling)
                {
                    // Prevent dungeon to disable all tiles and re-enable them a bit later when player spawn
                    adjacentRoomCulling.TargetOverride = playerStart;
                }
            }
            else
            {
                Debug.LogError("No player spawner found for dungeon!");
            }
        }
#endif
    }
}
