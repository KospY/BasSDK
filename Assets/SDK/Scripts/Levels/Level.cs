using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Text;

#if DUNGEN
using DunGen;
using DunGen.Adapters;
using UnityEngine.AI;
using UnityEngine.XR;
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
        public Transform playerStart;
        public List<CustomReference> customReferences;

        [NonSerialized]
        public State state = State.None;
        [NonSerialized]
        public AudioSource music;
        [NonSerialized]
        public bool loaded;

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
        public bool dungeonCullAdjacentRoom = true;
        [NonSerialized]
        public RuntimeDungeon dungeonGenerator;
        [NonSerialized]
        public UnityNavMeshAdapter dungeonNavMeshAdapter;
#endif

        [Button]
        public static void CheckLightMapMode()
        {
            Debug.Log("Lightmap mode: " + LightmapSettings.lightmapsMode);
        }

        [Button]
        public static void TetrahedralizeLightProbes()
        {
            // Fix light probes being wrong!!!
            LightProbes.Tetrahedralize();
        }

        protected virtual void Awake()
        {
            if (gameObject.scene.name.ToLower() != "master")
            {
                current = this;
            }
            if (!playerStart)
            {
                PlayerSpawner playerSpawner = GameObject.FindObjectOfType<PlayerSpawner>();
                if (playerSpawner)
                {
                    playerStart = playerSpawner.transform;
                }
            }
#if DUNGEN
            dungeonNavMeshAdapter = GameObject.FindObjectOfType<UnityNavMeshAdapter>();
            if (dungeonNavMeshAdapter)
            {
                dungeonNavMeshAdapter.enabled = false;
            }
            dungeonGenerator = GameObject.FindObjectOfType<RuntimeDungeon>();
            if (dungeonGenerator)
            {
                dungeonGenerator.Generator.OnGenerationStatusChanged += OnGenerationStatusChanged;
                GenerateDungeon();
            }
#endif
        }
#if DUNGEN
        [Button]
        public void GenerateDungeon()
        {
            if (dungeonGenerator && Application.isPlaying) dungeonGenerator.Generate();
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

            PlayerSpawner playerSpawner = GameObject.FindObjectOfType<PlayerSpawner>();
            if (playerSpawner)
            {
                playerStart = playerSpawner.transform;
                PlayerControllerTest playerControllerTest = GameObject.FindObjectOfType<PlayerControllerTest>();
                if (playerControllerTest)
                {
                    playerControllerTest.transform.SetPositionAndRotation(playerStart.position, playerStart.rotation);
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
