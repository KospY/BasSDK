using UnityEngine;
using System.Collections.Generic;
using System;

#if PrivateSDK
using Crest;
#endif

#if DUNGEN
using UnityEngine.SceneManagement;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Ocean")]
	[AddComponentMenu("ThunderRoad/Levels/Ocean")]
    public class Ocean : MonoBehaviour
    {
        public string prefabAddress = "Bas.Ocean.Greenland.LightHouse";
        public string lowQualityPrefabAddress = "Bas.Ocean.LowQuality";
        public GameObject lowQuality;
        public bool showWhenInRoomOnly = true;

#if PrivateSDK

        public static List<Ocean> all = new List<Ocean>();
        public static Ocean current;
        public static bool exist;

        [NonSerialized]
        public static Quality quality = Quality.Waves;
        public static event Action<Quality> qualityChangeEvent;

        [NonSerialized]
        public GameObject oceanGameobject;

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
            if (lowQuality) lowQuality.SetActive(false);
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
                crestOceanRenderer.Viewpoint = crestOceanRenderer.ViewCamera.transform;
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
                if (ocean.oceanGameobject && !ocean.lowQuality) Catalog.ReleaseAsset(ocean.oceanGameobject);
                ocean.crestOceanRenderer = null;
                ocean.oceanGameobject = null;
                if (ocean.isActiveAndEnabled) ocean.SetActive(true);
            }

            if(qualityChangeEvent != null) qualityChangeEvent.Invoke(quality);
        }

#if UNITY_EDITOR
        [Button("Spawn low quality water")]
        protected void EditorSpawnLowQuality()
        {
            if (oceanGameobject && !lowQuality) DestroyImmediate(oceanGameobject);
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
                    if (lowQuality)
                    {
                        oceanGameobject = lowQuality;
                        lowQuality.SetActive(true);
                        exist = true;
                        current = this;
                    }
                    else
                    {
                        spawning = true;
                        Catalog.InstantiateAsync(lowQualityPrefabAddress, this.transform.position, this.transform.rotation, null, go =>
                        {
                            MeshRenderer oceanMeshRenderer = go.GetComponent<MeshRenderer>();
                            if (oceanMeshRenderer)
                            {
#if UNITY_EDITOR
                                oceanMeshRenderer.transform.SetAsFirstSibling();
#endif
                                oceanGameobject = oceanMeshRenderer.gameObject;
                                SceneManager.MoveGameObjectToScene(oceanGameobject, this.gameObject.scene);
                                exist = true;
                                current = this;
                                spawning = false;
                            }
                        }, "OceanSpawner");
                    }
                }
                else if (quality == Quality.Waves)
                {
                    spawning = true;
                    Catalog.InstantiateAsync(prefabAddress, this.transform.position, this.transform.rotation, null, go =>
                    {
                        oceanGameobject = go;
#if UNITY_EDITOR
                        oceanGameobject.transform.SetAsFirstSibling();
#endif
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

                            if (PlayerTest.local)
                            {
                                crestOceanRenderer.ViewCamera = PlayerTest.local.cam;
                                crestOceanRenderer.Viewpoint = crestOceanRenderer.ViewCamera.transform;
                            }
                            current = this;
                            exist = true;
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
                    exist = true;
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
                exist = false;
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
                            current = ocean;
                            exist = true;
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
            if (oceanGameobject && !lowQuality) Catalog.ReleaseAsset(oceanGameobject);
        }
#endif
    }
}