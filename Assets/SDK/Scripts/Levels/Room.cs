using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;

#if DUNGEN
using DunGen;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class Room : MonoBehaviour
    {
        [Header("Baking"), Range(0, 5)]
        public float indirectIntensity = 1;
        public bool autoRotateDirLight;
        [ReadOnly]
        public Quaternion directionalLightRotation;
        protected Light directionalLight;

        [Header("Culling")]
        public float spawnRoomObjectsAcrossFrames = 2;

        [Header("NPC")]
        public int spawnerMaxNPC = 4;

        public Light GetDirectionalLight()
        {
            foreach (Light light in GameObject.FindObjectsOfType<Light>())
            {
                if (light.type == LightType.Directional && light.gameObject.scene == this.gameObject.scene)
                {
                    return light;
                }
            }
            Debug.LogError("No directional light found in the scene!");
            return null;
        }

        [Button]
        public void SaveSceneDirectionalLightRotation()
        {
            directionalLight = GetDirectionalLight();
            if (directionalLight)
            {
                directionalLightRotation = directionalLight.transform.rotation;
                Debug.Log("Directional light rotation saved!");
            }
        }

        public void SetSceneDirectionalLightRotation()
        {
            if (!directionalLight) directionalLight = GetDirectionalLight();
            if (directionalLight && directionalLightRotation != Quaternion.identity)
            {
                Debug.Log("Directional light rotation set by room " + this.name);
                directionalLight.transform.rotation = directionalLightRotation;
            }
        }

#if DUNGEN

        [Header("Culling")]
        [NonSerialized, ShowInInspector, ReadOnly]
        public List<AudioSource> audioSources;

        [NonSerialized, ShowInInspector, ReadOnly]
        public List<ReflectionProbe> reflectionProbes = new List<ReflectionProbe>();

        [NonSerialized, ShowInInspector, ReadOnly]
        public List<Doorway> doorways = new List<Doorway>();

        [NonSerialized, ShowInInspector, ReadOnly]
        public List<RoomObject> roomObjects = new List<RoomObject>();

        [NonSerialized, ShowInInspector, ReadOnly]
        public int spawnerNPCCount;

        [NonSerialized, ShowInInspector, ReadOnly]
        public bool isCulled;

        [NonSerialized]
        public DunGen.Tile tile;

        [NonSerialized, ShowInInspector, ReadOnly]
        public List<PlayerSpawner> playerSpawners = new List<PlayerSpawner>();

        [Header("Event")]
        public UnityEvent onPlayerEnter = new UnityEvent();
        public UnityEvent onPlayerExit = new UnityEvent();

        public delegate void VisibilityChangeEvent();
        public event VisibilityChangeEvent onVisibilityChange;

        [NonSerialized]
        public GameObject rootNoCulling;

        protected Coroutine enableRoomObjectCoroutine;

        private void OnValidate()
        {
            if (!this.gameObject.activeInHierarchy) return;
            if (this.gameObject.scene.name == null) return;
            if (!Application.isPlaying)
            {
                if (autoRotateDirLight) SetSceneDirectionalLightRotation();
            }
        }

        private void Awake()
        {
            directionalLight = GetDirectionalLight();
            tile = this.GetComponent<DunGen.Tile>();
            playerSpawners = new List<PlayerSpawner>(this.GetComponentsInChildren<PlayerSpawner>(true));
            audioSources = new List<AudioSource>(this.GetComponentsInChildren<AudioSource>());

            rootNoCulling = new GameObject("NoCulling");
            rootNoCulling.transform.SetParentOrigin(this.transform);

            Level.current.dungeon.onDungeonGenerated += OnDungeonGenerated;

            // Reflection probes
            GameObject rootReflectionProbes = new GameObject("ReflectionProbes");
            rootReflectionProbes.transform.SetParentOrigin(rootNoCulling.transform);
            foreach (ReflectionProbe reflectionProbe in this.GetComponentsInChildren<ReflectionProbe>(true))
            {
                // Isolate reflection probes as disabling them clear texture
                reflectionProbe.transform.SetParent(rootReflectionProbes.transform, true);
                InitReflectionProbeRotation(reflectionProbe);
                reflectionProbes.Add(reflectionProbe);
            }
            RenderProbes();

            // Light volume
            GameObject rootLightProbeVolumes = new GameObject("LightProbeVolumes");
            rootLightProbeVolumes.transform.SetParentOrigin(rootNoCulling.transform);
            foreach (RainyReignGames.PrefabBakedLighting.LightProbeVolume lightProbeVolume in this.GetComponentsInChildren<RainyReignGames.PrefabBakedLighting.LightProbeVolume>(true))
            {
                // Isolate probe volume to avoid separating them from script (colliders being moved after)
                lightProbeVolume.transform.SetParent(rootLightProbeVolumes.transform, true);
            }

            // Nav mesh link
            GameObject rootDoorWays = new GameObject("DoorWays");
            rootDoorWays.transform.SetParentOrigin(rootNoCulling.transform);
            foreach (Doorway doorway in this.GetComponentsInChildren<Doorway>(true))
            {
                // Isolate door way to move nav mesh link as enabling them cost a bit and we may need them for AI movement outside view later
                doorway.transform.SetParent(rootDoorWays.transform, true);
                doorways.Add(doorway);
            }
        }

        private void Start()
        {
            // Copy all colliders to a dedicated gameobject and disable original ones
            GameObject rootStaticColliders = new GameObject("StaticColliders");
            rootStaticColliders.transform.SetParentOrigin(rootNoCulling.transform);
            foreach (Transform transform in this.transform)
            {
                if (transform == rootNoCulling.transform) continue;

                foreach (Collider collider in transform.GetComponentsInChildren<Collider>())
                {
                    if (!collider.gameObject.activeInHierarchy) continue;
                    if (collider.gameObject.GetComponentInParent<Rigidbody>()) continue;
                    // Isolate colliders as disabling them is costly
                    GameObject colGo = new GameObject(collider.name);
                    collider.Clone(colGo);
                    colGo.layer = collider.gameObject.layer;
                    colGo.transform.SetParent(rootStaticColliders.transform);
                    colGo.transform.position = collider.transform.position;
                    colGo.transform.rotation = collider.transform.rotation;
                    colGo.transform.SetGlobalScale(collider.transform.lossyScale);
                    Destroy(collider);
                }
            }

            if (Level.current.dungeon.staticBatchRooms)
            {
                StaticBatch();
            }
        }

        public PlayerSpawner GetPlayerSpawner()
        {
            if (playerSpawners.Count == 0) return null;
            PlayerSpawner playerSpawner = playerSpawners[UnityEngine.Random.Range(0, playerSpawners.Count)];
            return playerSpawner;
        }

        public bool Contains(Vector3 position)
        {
            return tile.Bounds.Contains(position);
        }

        public void InitReflectionProbeRotation(ReflectionProbe reflectionProbe)
        {
            // Keep the original center and size if you want to use more than one time
            Bounds bounds = new Bounds(reflectionProbe.center, reflectionProbe.size);

            Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, transform.rotation, transform.localScale);
            Vector3 newMax = mat.MultiplyPoint3x4(bounds.max);
            Vector3 newMin = mat.MultiplyPoint3x4(bounds.min);

            bounds = new Bounds();
            bounds.Encapsulate(newMax);
            bounds.Encapsulate(newMin);

            reflectionProbe.center = bounds.center;
            reflectionProbe.size = bounds.size;
        }

        [Button]
        public void StaticBatch()
        {
            List<GameObject> objectsToBatch = new List<GameObject>();
            foreach (MeshRenderer meshRenderer in this.gameObject.GetComponentsInChildren<MeshRenderer>(true))
            {
                if (meshRenderer.lightmapIndex >= 0) // We can't check if static is checked at runtime, so check if lightmap exist
                {
                    objectsToBatch.Add(meshRenderer.gameObject);
                }
            }
            StaticBatchingUtility.Combine(objectsToBatch.ToArray(), this.gameObject);
            Debug.Log("Static batched " + objectsToBatch.Count + " objects for room " + this.name);
        }

        [Button]
        public void RenderProbes()
        {
            foreach (ReflectionProbe reflectionProbe in this.GetComponentsInChildren<ReflectionProbe>(true))
            {
                // Force Custom parameters
                reflectionProbe.mode = UnityEngine.Rendering.ReflectionProbeMode.Custom;
                reflectionProbe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;
                reflectionProbe.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
                // Capture cubemap
                reflectionProbe.RenderProbe();
            }
        }
        protected void OnDungeonGenerated()
        {
            foreach (ReflectionProbe reflectionProbe in this.GetComponentsInChildren<ReflectionProbe>(true))
            {
                reflectionProbe.customBakedTexture = reflectionProbe.realtimeTexture;
            }
        }

        public void OnPlayerEnter()
        {
            if (autoRotateDirLight)
            {
                SetSceneDirectionalLightRotation();
            }
            onPlayerEnter.Invoke();
        }

        public void OnPlayerExit()
        {
            onPlayerExit.Invoke();
        }

        public void RegisterObject(RoomObject roomObject)
        {
            if (!roomObjects.Contains(roomObject)) roomObjects.Add(roomObject);
        }

        public void UnRegisterObject(RoomObject roomObject)
        {
            if (roomObjects.Contains(roomObject)) roomObjects.Remove(roomObject);
        }

        public void SetCull(bool cull)
        {
            if (cull && !isCulled)
            {
                isCulled = true;
                foreach (Transform transform in this.transform)
                {
                    if (transform == rootNoCulling.transform) continue;
                    transform.gameObject.SetActive(false);
                }
                foreach (Doorway doorway in doorways)
                {
                    foreach (Transform blocker in doorway.transform)
                    {
                        blocker.gameObject.SetActive(false);
                    }
                }
                if (enableRoomObjectCoroutine != null) StopCoroutine(enableRoomObjectCoroutine);
                foreach (RoomObject roomObject in roomObjects)
                {
                    roomObject.SetCull(true);
                }
                if (onVisibilityChange != null) onVisibilityChange.Invoke();
                Level.current.dungeon.InvokeRoomVisibilityChange(this);
            }
            else if (!cull && isCulled)
            {
                isCulled = false;
                foreach (Transform transform in this.transform)
                {
                    if (transform == rootNoCulling.transform) continue;
                    transform.gameObject.SetActive(true);
                }
                foreach (Doorway doorway in doorways)
                {
                    foreach (Transform blocker in doorway.transform)
                    {
                        blocker.gameObject.SetActive(true);
                    }
                }
                if (enableRoomObjectCoroutine != null) StopCoroutine(enableRoomObjectCoroutine);
                if (spawnRoomObjectsAcrossFrames > 0)
                {
                    enableRoomObjectCoroutine = StartCoroutine(EnableRoomObjectCoroutine());
                }
                else
                {
                    foreach (RoomObject roomObject in roomObjects)
                    {
                        roomObject.SetCull(false);
                    }
                }
                if (onVisibilityChange != null) onVisibilityChange.Invoke();
                Level.current.dungeon.InvokeRoomVisibilityChange(this);
            }
        }

        IEnumerator EnableRoomObjectCoroutine()
        {
            for (int i = roomObjects.Count - 1; i >= 0; i--)
            {
                roomObjects[i].SetCull(false);
                for (int i2 = 0; i2 < spawnRoomObjectsAcrossFrames; i2++)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            enableRoomObjectCoroutine = null;
        }
#endif
    }
}