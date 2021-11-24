using UnityEngine;
using System.Collections.Generic;
using System;

#if PrivateSDK
using Crest;
#endif

#if DUNGEN
using DunGen;
using UnityEngine.SceneManagement;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Levels/Ocean")]
    public class Ocean : MonoBehaviour
    {
        public string prefabAddress = "Bas.Ocean.Greenland.LightHouse";
        public string lowQualityPrefabAddress = "Bas.Ocean.LowQuality";
        public bool showWhenInRoomOnly = true;

#if PrivateSDK

        public static List<Ocean> all = new List<Ocean>();
        public static Ocean current;
        protected static Quality quality = Quality.Waves;

        [NonSerialized]
        public GameObject oceanGameobject;

        [NonSerialized]
        public MeshRenderer oceanLowQualityMeshRenderer;

        [NonSerialized]
        public OceanRenderer crestOceanRenderer;
        [NonSerialized]
        public OceanDepthCache crestOceanDepthCache;
        [NonSerialized]
        public ShapeGerstner crestShapeGerstner;
        [NonSerialized]
        public ShapeFFT crestShapeFFT;
        protected bool spawning;

        protected Room room;

        public enum Quality
        {
            Plane,
            Waves,
        }

        private void Awake()
        {
            room = this.GetComponentInParent<Room>();
            if (room)
            {
                room.onPlayerEnter.AddListener(OnPlayerEnterRoom);
                room.onPlayerExit.AddListener(OnPlayerExitRoom);
            }
            all.Add(this);
            if (Level.current && Level.current.dungeon)
            {
                Level.current.dungeon.onDungeonGenerated += OnDungeonGenerated;
            }
            else if (isActiveAndEnabled)
            {
                SetActive(true);
            }
            PlayerTest.onSpawn += OnPlayerTestSpawned;
        }

        private void OnDungeonGenerated(EventTime eventTime)
        {
            if (eventTime == EventTime.OnEnd && isActiveAndEnabled)
            {
                if (!showWhenInRoomOnly || !room || Dungeon.playerRoom == room)
                {
                    SetActive(true);
                }
            }
        }

        private void OnPlayerTestSpawned(PlayerTest player)
        {
            if (crestOceanRenderer)
            {
                crestOceanRenderer.ViewCamera = player.cam;
            }
        }

        private void OnPlayerEnterRoom()
        {
            if (showWhenInRoomOnly && room)
            {
                SetActive(true);
            }
        }

        private void OnPlayerExitRoom()
        {
            if (showWhenInRoomOnly && room)
            {
                SetActive(false);
            }
        }

        public static void SetQuality(Quality quality)
        {
            Ocean.quality = quality;
            foreach (Ocean ocean in Ocean.all)
            {
                if (ocean.spawning) return;
                ocean.SetActive(false);
                if (ocean.oceanGameobject) Catalog.ReleaseAsset(ocean.oceanGameobject);
                ocean.oceanLowQualityMeshRenderer = null;
                ocean.crestOceanRenderer = null;
                ocean.oceanGameobject = null;
                if (ocean.isActiveAndEnabled) ocean.SetActive(true);
            }
        }

#if UNITY_EDITOR
        [Button("Spawn low quality water")]
        protected void EditorSpawnLowQuality()
        {
            if (oceanGameobject) DestroyImmediate(oceanGameobject);
            oceanGameobject = null;
            SetQuality(Quality.Plane);
            Spawn();
        }

        [Button("Spawn high quality water")]
        protected void EditorSpawnHighQuality()
        {
            if (oceanGameobject) DestroyImmediate(oceanGameobject);
            oceanGameobject = null;
            SetQuality(Quality.Waves);
            Spawn();
        }
#endif

        protected void Spawn()
        {
            if (!spawning && !oceanGameobject)
            {
                if (quality == Quality.Plane)
                {
                    spawning = true;
                    Catalog.InstantiateAsync<MeshRenderer>(lowQualityPrefabAddress, oceanMeshRenderer =>
                    {
                        if (oceanMeshRenderer)
                        {
                            oceanLowQualityMeshRenderer = oceanMeshRenderer;
                            oceanGameobject = oceanMeshRenderer.gameObject;
                            oceanGameobject.transform.position = this.transform.position;
                            oceanGameobject.transform.rotation = this.transform.rotation;
                            SceneManager.MoveGameObjectToScene(oceanGameobject, this.gameObject.scene);
                            spawning = false;
                        }
                    }, "OceanSpawner");
                }
                else if (quality == Quality.Waves)
                {
                    spawning = true;
                    Catalog.InstantiateAsync<GameObject>(prefabAddress, go =>
                    {
                        oceanGameobject = go;
                        oceanGameobject.transform.position = this.transform.position;
                        oceanGameobject.transform.rotation = this.transform.rotation;
                        crestOceanRenderer = oceanGameobject.GetComponentInChildren<OceanRenderer>();
                        if (crestOceanRenderer)
                        {
                            crestOceanRenderer.transform.position = this.transform.position;
                            crestOceanRenderer.transform.rotation = Quaternion.identity;
                            SceneManager.MoveGameObjectToScene(oceanGameobject, this.gameObject.scene);
                            crestOceanDepthCache = oceanGameobject.GetComponentInChildren<OceanDepthCache>();
                            crestShapeFFT = oceanGameobject.GetComponentInChildren<ShapeFFT>();
                            crestShapeGerstner = oceanGameobject.GetComponentInChildren<ShapeGerstner>();

                            if (crestShapeGerstner)
                            {
                                crestShapeGerstner._waveDirectionHeadingAngle -= this.transform.root.eulerAngles.y;
                                if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
                                {
                                    Debug.LogError("Disabled crest ocean shapeGerstner as not supported on OpenGLES3");
                                    crestShapeGerstner.enabled = false;
                                }
                            }

                            if (crestShapeFFT)
                            {
                                crestShapeFFT._waveDirectionHeadingAngle -= this.transform.root.eulerAngles.y;
                            }

                            if (PlayerTest.local) crestOceanRenderer.ViewCamera = PlayerTest.local.cam;
                            spawning = false;
                        }
                    }, "OceanSpawner");
                }
            }
        }

        private void OnEnable()
        {
            if (!showWhenInRoomOnly || !room || Dungeon.playerRoom == room)
            {
                SetActive(true);
            }
        }

        private void OnDisable()
        {
            SetActive(false);
        }

        public void SetActive(bool active)
        {
            if (Level.current && Level.current.dungeon && !Level.current.dungeon.initialized) return;
            if (active)
            {
                // Disable other oceans if any
                foreach (Ocean ocean in all)
                {
                    if (ocean != this && ocean.isActiveAndEnabled)
                    {
                        if (ocean.spawning) return;
                        if (ocean.oceanGameobject)
                        {
                            ocean.oceanGameobject.SetActive(false);
                        }
                    }
                }
                if (this.oceanGameobject)
                {
                    this.oceanGameobject.SetActive(true);
                }
                else
                {
                    Spawn();
                }
                current = this;
            }
            else
            {
                current = null;
                if (this.oceanGameobject)
                {
                    this.oceanGameobject.SetActive(false);
                }
                // Enable other oceans if any
                foreach (Ocean ocean in all)
                {
                    if (ocean != this && ocean.isActiveAndEnabled)
                    {
                        if (ocean.oceanGameobject)
                        {
                            ocean.oceanGameobject.SetActive(true);
                            current = this;
                            break;
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            PlayerTest.onSpawn -= OnPlayerTestSpawned;
            SetActive(false);
            all.Remove(this);
            if (oceanGameobject) Catalog.ReleaseAsset(oceanGameobject);
        }
#endif
    }
}