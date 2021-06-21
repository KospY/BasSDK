using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class Room : MonoBehaviour
    {
        [Header("Culling")]
        public List<GameObject> disableWhenCulled;
        [ReadOnly]
        public List<AudioSource> audioSources;
        [ReadOnly]
        public List<ReflectionProbe> reflectionProbes;

        [Header("Baking"), Range(0, 5)]
        public float indirectIntensity = 1;
        public bool autoRotateDirLight;
        [ReadOnly]
        public Quaternion directionalLightRotation;
        protected Light directionalLight;

        [Header("NPC")]
        public int spawnerMaxNPC = 4;

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public int spawnerNPCCount;

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public bool isCulled;

#if DUNGEN
        [NonSerialized]
        public DunGen.Tile tile;
#endif

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public List<GameObject> disabledChilds = new List<GameObject>();

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public List<PlayerSpawner> playerSpawners = new List<PlayerSpawner>();

        [Header("Event")]
        public UnityEvent onPlayerEnter = new UnityEvent();
        public UnityEvent onPlayerExit = new UnityEvent();

        public class RoomVisibilityEvent : UnityEvent<bool> { }
        public RoomVisibilityEvent onRoomVisibilityChange = new RoomVisibilityEvent();

        private void OnValidate()
        {
            audioSources = new List<AudioSource>(this.GetComponentsInChildren<AudioSource>());
            reflectionProbes = new List<ReflectionProbe>(this.GetComponentsInChildren<ReflectionProbe>());
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
#if DUNGEN
            tile = this.GetComponent<DunGen.Tile>();
#endif
            foreach (LightProbeGroup lightProbeGroup in this.GetComponentsInChildren<LightProbeGroup>())
            {
                lightProbeGroup.enabled = false;
            }
            playerSpawners = new List<PlayerSpawner>(this.GetComponentsInChildren<PlayerSpawner>(true));
        }

        private void Start()
        {
            RefreshReflectionProbeRotation();
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

        [Button]
        public bool Contains(Vector3 position)
        {
            return tile.Bounds.Contains(position);
        }

        [Button]
        public void RefreshReflectionProbeRotation()
        {
            foreach (ReflectionProbe reflectionProbe in reflectionProbes)
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

        public void SetCull(bool cull)
        {
            if (cull && !isCulled)
            {
                disabledChilds.Clear();
                foreach (Transform transform in this.transform)
                {
                    if (transform.gameObject.activeInHierarchy)
                    {
                        transform.gameObject.SetActive(false);
                        disabledChilds.Add(transform.gameObject);
                    }
                }
                isCulled = true;
                onRoomVisibilityChange.Invoke(isCulled);
                Level.current.dungeon.onRoomVisibilityChange.Invoke(this);
            }
            else if (!cull && isCulled)
            {
                foreach (GameObject disabledChild in disabledChilds)
                {
                    disabledChild.SetActive(true);
                }
                disabledChilds.Clear();
                isCulled = false;
                onRoomVisibilityChange.Invoke(isCulled);
                Level.current.dungeon.onRoomVisibilityChange.Invoke(this);
            }
        }

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
    }
}