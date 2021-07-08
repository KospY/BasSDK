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

#if PrivateSDK

        public static List<Ocean> all = new List<Ocean>();
        public static Ocean current;

        [NonSerialized]
        public OceanRenderer crestOceanRenderer;
        [NonSerialized]
        public OceanDepthCache crestOceanDepthCache;
        [NonSerialized]
        public ShapeGerstner crestShapeGerstner;
        protected bool spawning;

        private void Awake()
        {
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
                SetActive(true);
            }
        }

        private void OnPlayerTestSpawned(PlayerTest player)
        {
            if (crestOceanRenderer)
            {
                crestOceanRenderer.ViewCamera = player.cam;
            }
        }

        protected void Spawn()
        {
            if (!spawning && !crestOceanRenderer)
            {
                spawning = true;
                Catalog.InstantiateAsync<OceanRenderer>(prefabAddress, oceanRenderer =>
                {
                    if (oceanRenderer)
                    {
                        crestOceanRenderer = oceanRenderer;
                        crestOceanRenderer.transform.position = this.transform.position;
                        crestOceanRenderer.transform.rotation = Quaternion.identity;
                        SceneManager.MoveGameObjectToScene(crestOceanRenderer.gameObject, this.gameObject.scene);

                        crestOceanDepthCache = crestOceanRenderer.GetComponentInChildren<OceanDepthCache>();
                        if (crestOceanDepthCache)
                        {
                            crestOceanRenderer.transform.rotation = Quaternion.Euler(0, this.transform.eulerAngles.y, 0);
                            crestOceanDepthCache.transform.SetParent(null, true);
                            crestOceanRenderer.transform.rotation = Quaternion.identity;
                            crestOceanDepthCache.transform.SetParent(crestOceanRenderer.transform, true);
                            crestOceanDepthCache.enabled = true;
                        }

                        crestShapeGerstner = crestOceanRenderer.GetComponent<ShapeGerstner>();
                        if (crestShapeGerstner)
                        {
                            crestShapeGerstner._waveDirectionHeadingAngle = this.transform.eulerAngles.y - 90;
                            if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
                            {
                                Debug.LogError("Disabled crest ocean shapeGerstner as not supported on OpenGLES3");
                                crestShapeGerstner.enabled = false;
                            }
                        }

                        if (PlayerTest.local) crestOceanRenderer.ViewCamera = PlayerTest.local.cam;
                        spawning = false;
                    }
                }, "OceanSpawner");
            }
        }

        private void OnEnable()
        {
            SetActive(true);
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
                        if (ocean.crestOceanRenderer)
                        {
                            ocean.crestOceanRenderer.gameObject.SetActive(false);
                        }
                    }
                }
                if (this.crestOceanRenderer)
                {
                    this.crestOceanRenderer.gameObject.SetActive(true);
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
                if (this.crestOceanRenderer)
                {
                    this.crestOceanRenderer.gameObject.SetActive(false);
                }
                // Enable other oceans if any
                foreach (Ocean ocean in all)
                {
                    if (ocean != this && ocean.isActiveAndEnabled)
                    {
                        if (ocean.crestOceanRenderer)
                        {
                            ocean.crestOceanRenderer.gameObject.SetActive(true);
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
            if (crestOceanRenderer)
            {
                Destroy(crestOceanRenderer.gameObject);
            }
        }
#endif
    }
}