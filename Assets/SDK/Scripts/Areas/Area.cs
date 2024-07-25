using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;
using System.Linq;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Areas/Area.html")]
    [RequireComponent(typeof(AreaLightingGroupLiteMemoryToggle))] // The lighting preset should always be loaded by addressables via the AreaLightingGroupLiteMemoryToggle
    public class Area : MonoBehaviour, ICheckAsset
    {
        #region InternalClass
        [Serializable]
        public class CustomReference
        {
            public string name;
            public List<Transform> transforms;
        }

        public class BlendAudioSource
        {
            private AudioSource audioSource;
            private FxModuleAudio audioFx;
            private float orgVolume;
            public BlendAudioSource(AudioSource audioSource)
            {
                FxModuleAudio audioFx = audioSource.GetComponentInParent<FxModuleAudio>();
                if (audioFx != null && audioFx.audioSource == audioSource)
                {
                    this.audioFx = audioFx;
                    this.orgVolume = EffectAudio.DecibelToLinear(audioFx.volumeDb);
                    this.audioSource = null;
                    return;
                }

                this.audioFx = null;
                this.audioSource = audioSource;
                this.orgVolume = audioSource.volume;
            }

            /// <summary>
            /// Apply volume as lerp between 0 and original volume
            /// </summary>
            /// <param name="t"> t the lerp parameter (t = 1.0f will apply original volume)</param>
            public void ApplyVolume(float t)
            {
                float newVolume = Mathf.Lerp(0, this.orgVolume, t);
                if (audioFx != null)
                {
                    audioFx.volumeDb = EffectAudio.LinearToDecibel(newVolume);
                    audioFx.Refresh();
                    return;
                }

                audioSource.volume = newVolume;
            }
        }

        [Serializable]
        public class MeshColliderRef
        {
            public bool convex;
            public Mesh mesh;
        }

        public struct BakeJob : IJobParallelFor
        {
            // see doc : https://docs.unity3d.com/2019.4/Documentation/ScriptReference/Physics.BakeMesh.html
            public const int MESH_PER_JOB = 10;
            private NativeArray<int> meshIds;
            private NativeArray<bool> meshConvex;

            public BakeJob(NativeArray<int> meshIds, NativeArray<bool> meshConvex)
            {
                this.meshIds = meshIds;
                this.meshConvex = meshConvex;
            }

            public void Execute(int index)
            {
                Physics.BakeMesh(meshIds[index], meshConvex[index]);
            }
        }

        #endregion InternalClass

        #region Fields
        #region SerializedFields        
#if UNITY_EDITOR
        [Header("CatalogData")]
        [Tooltip("Contains the Area ID")]
        public DataIdContainer<AreaData> areaData;
        public int lightingPresetIndex = 0;

        [Header("Tool boundary")]
        public bool boundIncludeInactive;
        public bool boundIncludeDisableOnPlay;

        [Tooltip("Ignores the reference mesh when a boundery bounds is calculated")]
        public Renderer[] boundRendererToIgnore;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public bool modifiedInImport = false;
#endif //UNITY_EDITOR

        [Header("Custom References")]
        public List<CustomReference> customReferences;

        [Header("Culling")]
#if ODIN_INSPECTOR
        [InlineButton("UpdateCullingVolume", "Update Volume")]
#endif
        [Tooltip("Spawns the area across a number of frames during load time (Recommended 2)")]
        public float spawnRoomObjectsAcrossFrames = 2;

        [Header("Audio blend")]
#if ODIN_INSPECTOR
        [InlineButton("AddLoopedAudioSources")]
#endif
        [Tooltip("Lists looping audio sources which will blend between eachother.")]
        public List<AudioSource> audioSourcesToBlend = new List<AudioSource>();

        [Tooltip("This is the root for everyting that should not be disabled when culled")]
        public GameObject rootNoCulling;

        // Hiden SerializedFields
        [HideInInspector]
        public LightingGroup lightingGroup;
        [HideInInspector]
        public ListMonoBehaviourReference<ILiteMemoryToggle> liteMemoryToggles;
        [NonSerialized]
        public bool lightingPresetFromData;
        [HideInInspector]
        public UnityEngine.VFX.VisualEffect[] visualEffects;
        [HideInInspector]
        public ParticleSystem[] particlesSystem;
        [HideInInspector]
        public AreaGateway[] gateways;
        [HideInInspector]
        public List<PlayerSpawner> playerSpawners = new List<PlayerSpawner>();
        [HideInInspector]
        public ItemSpawnerLimiter itemLimiterSpawner = null;
        [HideInInspector]
        public List<CreatureSpawner> creatureSpawners = new List<CreatureSpawner>();
        [HideInInspector]
        public List<CreatureSpawner> creatureNoLimiteSpawners = new List<CreatureSpawner>();
        [HideInInspector]
        public ReflectionProbe[] reflectionProbes;
        [HideInInspector]
        public MeshRenderer[] meshRenderers;
        [HideInInspector]
        public List<MeshColliderRef> meshColliderRefList = new List<MeshColliderRef>();
        [HideInInspector]
        public List<Collider> colliderList = new List<Collider>();
        #endregion SerializedFields

        #region NonSerializedFields
#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public Dictionary<int, AreaGateway> connectedGateways = new Dictionary<int, AreaGateway>();

#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public List<GameObject> blockers = new List<GameObject>();

#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public bool initialized;

        [Header("Culling")]
#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public List<BlendAudioSource> blendAudioSources;

#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public List<Item> items = new List<Item>();

#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public List<Creature> creatures = new List<Creature>();

#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public bool isCulled;

#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public bool isHidden;

        [NonSerialized]
        public SpawnableArea spawnableArea;

        [NonSerialized]
        public bool IsActive = false;

        protected Coroutine toggleRoomObjectCoroutine;


        #endregion NonSerializedFields
        #endregion Fields

        #region Properties
        public bool IsInitialized { get { return initialized; } }
        #endregion Properties

        #region Event
        [Header("Event")]
        public UnityEvent onPlayerEnter = new UnityEvent();
        public UnityEvent onPlayerExit = new UnityEvent();
        public UnityEvent onInitialized = new UnityEvent();

        public delegate void CullChangeEvent(bool isCulled);
        public event CullChangeEvent onCullChange;

        public delegate void HideChangeEvent(bool isHide);
        public event HideChangeEvent onHideChange;
        #endregion Event

        #region Methods

        /// <summary>
        /// The layers which will be rendered by the reflection probes
        /// </summary>
        /// <returns></returns>
        private int GetReflectionLayers()
        {
            int reflectionLayers = 0;
            reflectionLayers |= 1 << LayerMask.NameToLayer("Default");
            reflectionLayers |= 1 << LayerMask.NameToLayer("Water");
            reflectionLayers |= 1 << LayerMask.NameToLayer("LocomotionOnly");
            reflectionLayers |= 1 << LayerMask.NameToLayer("NoLocomotion");
            reflectionLayers |= 1 << LayerMask.NameToLayer("SkyDome");
            return reflectionLayers;
        }

        protected bool LODHaveRenderers(LOD lod)
        {
            int rendererCount = lod.renderers.Length;
            for (int i = 0; i < rendererCount; i++)
            {
                if (lod.renderers[i] != null)
                {
                    return true;
                }
            }
            return false;
        }

        protected bool IsAdditionalNonLOD0Collider(LODGroup lODGroup, Collider collider)
        {
            LOD[] lods = lODGroup.GetLODs();

            if (lods.Length == 0)
                return false;
            if (!LODHaveRenderers(lods[0]))
                return false;

            for (int indexLods = 1; indexLods < lods.Length; indexLods++)
            {
                Renderer[] renderers = lods[indexLods].renderers;
                int rendererCount = renderers.Length;
                for (int indexRenderer = 0; indexRenderer < rendererCount; indexRenderer++)
                {
                    Renderer renderer = renderers[indexRenderer];
                    if (!renderer)
                        continue;

                    Collider[] lodColliders = renderer.gameObject.GetComponentsInChildren<Collider>();
                    int lodColliderCount = lodColliders.Length;
                    for (int indexLodCollider = 0; indexLodCollider < lodColliderCount; indexLodCollider++)
                    {
                        Collider lodCollider = lodColliders[indexLodCollider];
                        if (lodCollider == collider)
                            return true;
                    }
                }
            }
            return false;
        }
        #endregion Methods

        #region Editor
#if UNITY_EDITOR
        public void OnImport(UnityEditor.AssetPostprocessor postProcess)
        {
            Debug.LogFormat(this, $"Importing area {name}");
            try
            {
                lightingGroup = this.GetComponent<LightingGroup>();
                if (lightingGroup)
                {
                    //remove the reference to the lighting preset, as this should *always*  be loaded via addressables
                    lightingGroup.ClearOnImport();
                }
                else
                {
                    Debug.LogError($"Area {name} : lightingGroup is missing");
                }
            } catch (Exception e)
            {
                Debug.LogError($"Area Import {name} : {e.Message}");
            }


            try
            {
                ILiteMemoryToggle[] liteMemory = this.GetComponentsInChildren<ILiteMemoryToggle>(true);
                liteMemoryToggles = new ListMonoBehaviourReference<ILiteMemoryToggle>();
                for (int i = 0; i < liteMemory.Length; i++)
                {
                    liteMemoryToggles.Add(liteMemory[i]);
                }
            } catch (Exception e)
            {
                Debug.LogError($"Area Import {name} : {e.Message}");
            }


            visualEffects = GetComponentsInChildren<UnityEngine.VFX.VisualEffect>();
            particlesSystem = GetComponentsInChildren<ParticleSystem>();
            gateways = GetComponentsInChildren<AreaGateway>();

            playerSpawners = new List<PlayerSpawner>(this.GetComponentsInChildren<PlayerSpawner>(true));

            itemLimiterSpawner = GetComponent<ItemSpawnerLimiter>();
            if (itemLimiterSpawner == null)
            {
                //add the itemLimiterSpawner to the area
                itemLimiterSpawner = gameObject.AddComponent<ItemSpawnerLimiter>();
                itemLimiterSpawner.maxSpawn = 16;
                itemLimiterSpawner.maxChildSpawn = 8;
                itemLimiterSpawner.androidMaxSpawn = 6;
                itemLimiterSpawner.androidMaxChildSpawn = 2;
                itemLimiterSpawner.spawnOnStart = false;
                itemLimiterSpawner.spawnOnLevelLoad = false;
            }
            if (itemLimiterSpawner != null)
            {
                itemLimiterSpawner.spawnOnStart = false;
            }
            else
            {
                Debug.LogError($"No itemLimiterSpawner in area {gameObject.name}");
            }

            CreatureSpawner[] allCreatureSpawner = this.GetComponentsInChildren<CreatureSpawner>();
            creatureSpawners = new List<CreatureSpawner>();
            creatureNoLimiteSpawners = new List<CreatureSpawner>();
            for (int i = 0; i < allCreatureSpawner.Length; i++)
            {
                if (allCreatureSpawner[i].ignoredByAreas) continue;
                allCreatureSpawner[i].spawnOnStart = false;
                if (allCreatureSpawner[i].ignoreRoomMaxNPC)
                {
                    creatureNoLimiteSpawners.Add(allCreatureSpawner[i]);
                }
                else
                {
                    creatureSpawners.Add(allCreatureSpawner[i]);
                }
            }

            // Get and set reflection probes to baked
            List<ReflectionProbe> allReflectionProbes = new List<ReflectionProbe>();
            foreach (ReflectionProbe reflectionProbe in this.GetComponentsInChildren<ReflectionProbe>())
            {
                if (reflectionProbe.GetComponentInParent<Water>(true)) continue;
                allReflectionProbes.Add(reflectionProbe);
            }
            reflectionProbes = allReflectionProbes.ToArray();
            int reflectionLayers = GetReflectionLayers();
            foreach (ReflectionProbe reflectionProbe in reflectionProbes)
            {
                reflectionProbe.mode = UnityEngine.Rendering.ReflectionProbeMode.Baked;
                reflectionProbe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;
                //set the culling mask
                reflectionProbe.cullingMask = reflectionLayers;
                
            }

            //get all the PlayerLightMapVolumes and set their layer to "Ignore Raycast"
            PlayerLightMapVolume[] playerLightMapVolumes = GetComponentsInChildren<PlayerLightMapVolume>(true);
            foreach (PlayerLightMapVolume playerLightMapVolume in playerLightMapVolumes)
            {
                playerLightMapVolume.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
            meshRenderers = GetComponentsInChildren<MeshRenderer>(true);

            // Set collider to no culling
            if (rootNoCulling == null)
            {
                rootNoCulling = new GameObject("NoCulling");
                postProcess.context.AddObjectToAsset("NoCulling", rootNoCulling);
            }

            rootNoCulling.transform.SetParentOrigin(this.transform);

            // Copy all colliders to a dedicated gameobject and destroy original ones
            GameObject rootStaticColliders = new GameObject("StaticColliders");
            postProcess.context.AddObjectToAsset("StaticColliders", rootStaticColliders);

            rootStaticColliders.transform.SetParentOrigin(rootNoCulling.transform);

            meshColliderRefList.Clear();
            colliderList = new List<Collider>();
            HashSet<int> presentMesh = new HashSet<int>();

            int count = 0;
            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                if (!isActiveInArea(collider.gameObject))
                {
                    continue;
                }
                if (collider.isTrigger)
                {
                    continue;
                }
                if (!collider.enabled)
                {
                    continue;
                }
                if (collider.gameObject.GetComponentInParent<Rigidbody>(true))
                {
                    continue;
                }
                if (!collider.gameObject.isStatic)
                {
                    continue;
                }

                DisableOnCondition disableOnCondition = collider.gameObject.GetComponentInParent<DisableOnCondition>(true);
                if (disableOnCondition)
                {
                    if (disableOnCondition.condition == DisableOnCondition.Condition.OnPlay || disableOnCondition.condition == DisableOnCondition.Condition.OnRoomCulled)
                    {
                        continue;
                    }
                }

                if (collider.gameObject.GetComponentInParent<Zone>(true))
                {
                    continue;
                }


                GameObject colliderGo = collider.gameObject;

                // Destroy LOD1,2,3 colliders if any
                LODGroup lODGroup = collider.transform.GetComponentInParent<LODGroup>(true);
                if (lODGroup && IsAdditionalNonLOD0Collider(lODGroup, collider))
                {
                    //Destroy(collider);
                    collider.enabled = false;
                    continue;
                }

                // Isolate colliders as disabling them is costly
                GameObject colGo = new GameObject(collider.name);
                collider.Clone(colGo);
                colGo.name += $"({count})";
                colGo.layer = collider.gameObject.layer;
                colGo.transform.SetParent(rootStaticColliders.transform);
                colGo.transform.position = collider.transform.position;
                colGo.transform.rotation = collider.transform.rotation;
                colGo.transform.SetGlobalScale(collider.transform.lossyScale);
                //copy the tag as well so we dont lose important info
                colGo.tag = collider.gameObject.tag;
                //Destroy(collider);
                collider.enabled = false;

                Collider newCollider = colGo.GetComponent<Collider>();

                if (newCollider is MeshCollider meshCollider)
                {
                    meshCollider.enabled = false;
                    if (meshCollider.sharedMesh != null && presentMesh.Add(meshCollider.sharedMesh.GetInstanceID()))
                    {
                        MeshColliderRef meshColliderRef = new MeshColliderRef();
                        meshColliderRef.convex = meshCollider.convex;
                        Mesh mesh = meshCollider.sharedMesh;
                        meshColliderRef.mesh = mesh;
                        meshColliderRefList.Add(meshColliderRef);

                        if (!mesh.isReadable)
                        {
                            string meshPath = AssetDatabase.GetAssetPath(mesh);
                            if (ModelImporter.GetAtPath(meshPath) is ModelImporter meshImporter)
                            {
                                meshImporter.isReadable = true;
                                EditorUtility.SetDirty(meshImporter);
                                AssetDatabase.WriteImportSettingsIfDirty(meshPath);
                            }
                        }
                    }
                }

                colliderList.Add(newCollider);

                postProcess.context.AddObjectToAsset($"Collider{count}", newCollider.gameObject);
                count++;
            }

            // Isolate probe volume to avoid separating them from script 

            LightProbeVolume[] lightProbeVolumes = GetComponentsInChildren<LightProbeVolume>(true);
            GameObject rootLightProbeVolumes = new GameObject("LightProbeVolumes");
            rootLightProbeVolumes.transform.SetParentOrigin(rootNoCulling.transform);

            int lightProbeVolumesCount = lightProbeVolumes.Length;
            for (int i = 0; i < lightProbeVolumesCount; i++)
            {
                lightProbeVolumes[i].transform.SetParent(rootLightProbeVolumes.transform, true);
            }



            // Check That area audio loader on import has no audio preload 
            foreach (AudioLoader audioLoader in GetComponentsInChildren<AudioLoader>())
            {
                AudioClip audioClip = null;
                if (audioLoader.useAudioClipAddress)
                {
                    audioClip = Catalog.EditorLoad<AudioClip>(audioLoader.audioClipAddress);
                }
                else
                {
                    audioClip = audioLoader.audioClipReference.editorAsset;
                }

                if (audioClip != null)
                {
                    if (!audioClip.loadInBackground)
                    {
                        string audioPath = AssetDatabase.GetAssetPath(audioClip);
                        if (AudioImporter.GetAtPath(audioPath) is AudioImporter audioImporter)
                        {
                            audioImporter.loadInBackground = true;
                            EditorUtility.SetDirty(audioImporter);
                            AssetDatabase.WriteImportSettingsIfDirty(audioPath);
                        }
                    }
                }
            }
            
            //make sure any missing looped audio sources are added
            AddLoopedAudioSources();
            
            //mark the asset as dirty
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            modifiedInImport = true;
        }

        private bool isActiveInArea(GameObject go)
        {
            if (go == gameObject) return true;
            if (!go.activeSelf) return false;

            return isActiveInArea(go.transform.parent.gameObject);
        }

        public void ActiveCollider()
        {
            foreach (Collider collider in colliderList)
            {
                if (collider != null)
                {
                    collider.enabled = true;
                }
            }
        }

        public void DisableMeshCollider()
        {
            foreach (Collider collider in colliderList)
            {
                if (collider is MeshCollider)
                {
                    collider.enabled = false;
                }
            }
        }

        [Button]
        public void ResetImport()
        {
            ActiveCollider();

            lightingGroup = null;
            visualEffects = null;
            particlesSystem = null;
            gateways = null;
            playerSpawners = null;
            itemLimiterSpawner = null;
            creatureSpawners = null;
            reflectionProbes = null;
            meshRenderers = null;
            rootNoCulling = null;
            meshColliderRefList = null;
            colliderList = null;

        }

        [Button]
        public void AddSelectionToBoundRendererIgnorer()
        {
            HashSet<Renderer> tempHash = new HashSet<Renderer>();

            if (boundRendererToIgnore != null)
            {
                foreach (Renderer temp in boundRendererToIgnore)
                {
                    tempHash.Add(temp);
                }
            }

            foreach (GameObject go in Selection.gameObjects)
            {
                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    tempHash.Add(renderer);
                }

                Renderer[] childRenderer = go.GetComponentsInChildren<Renderer>();
                foreach (Renderer child in childRenderer)
                {
                    tempHash.Add(child);
                }

                Collider col = go.GetComponent<Collider>();
                if (col != null)
                {
                    Debug.LogError($"A collider is present in go : {go.name}");
                }

                Collider[] colChild = go.GetComponentsInChildren<Collider>();
                if (colChild.Length > 0)
                {
                    Debug.LogError($"A collider is present in go : {go.name}");
                }
            }

            boundRendererToIgnore = tempHash.ToArray();
        }

        [Button]
        public void UpdateDataBounds()
        {
            Catalog.EditorLoadAllJson();
            AreaData data = areaData.Data;
            if (data == null) return;

            Bounds bounds = new Bounds();
            bool hasBounds = false;

            // Renderers
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>(boundIncludeInactive))
            {
                if (boundRendererToIgnore != null)
                {
                    bool isIgnored = false;
                    for (int i = 0; i < boundRendererToIgnore.Length; i++)
                    {
                        if (renderer == boundRendererToIgnore[i])
                        {
                            isIgnored = true;
                            break;
                        }
                    }

                    if (isIgnored)
                    {
                        continue;
                    }
                }

                if (renderer is SpriteRenderer) continue;
                if (renderer is ParticleSystemRenderer) continue;
                if (!boundIncludeDisableOnPlay)
                {
                    DisableOnCondition disableOnCondition = renderer.GetComponentInParent<DisableOnCondition>();
                    if (disableOnCondition && disableOnCondition.condition == DisableOnCondition.Condition.OnPlay) continue;
                }
                if (renderer.gameObject.layer == 1) continue; // Ignore transparent VFX
                if (renderer.gameObject.layer == 2) continue; // igonore ignore raycast
                if (renderer.gameObject.layer == 4) continue; // Ignore water
                if (renderer.gameObject.layer == 5) continue; // Ignore UI

                if (hasBounds)
                {
                    bounds.Encapsulate(renderer.bounds);
                }
                else
                {
                    bounds = renderer.bounds;
                }
                hasBounds = true;
            }

            // Colliders
            ActiveCollider();
            foreach (Collider collider in GetComponentsInChildren<Collider>(boundIncludeInactive))
            {
                if (collider.isTrigger) continue;
                if (collider.gameObject.layer == 1) continue; // Ignore transparent VFX
                if (collider.gameObject.layer == 2) continue; // igonore ignore raycast
                if (collider.gameObject.layer == 4) continue; // Ignore water
                if (collider.gameObject.layer == 5) continue; // Ignore UI
                if (!boundIncludeDisableOnPlay)
                {
                    DisableOnCondition disableOnCondition = collider.GetComponentInParent<DisableOnCondition>();
                    if (disableOnCondition && disableOnCondition.condition == DisableOnCondition.Condition.OnPlay) continue;
                }
                if (hasBounds)
                {
                    bounds.Encapsulate(collider.bounds);
                }
                else
                {
                    bounds = collider.bounds;
                }
                hasBounds = true;
            }

            DisableMeshCollider();

            // Fix any zero or negative extents
            const float minExtents = 0.01f;
            Vector3 extents = bounds.extents;

            if (extents.x == 0f)
            {
                extents.x = minExtents;
            }
            else if (extents.x < 0f)
            {
                extents.x *= -1f;
            }

            if (extents.y == 0f)
            {
                extents.y = minExtents;
            }
            else if (extents.y < 0f)
            {
                extents.y *= -1f;
            }

            if (extents.z == 0f)
            {
                extents.z = minExtents;
            }
            else if (extents.z < 0f)
            {
                extents.z *= -1f;
            }

            bounds.extents = extents;
            data.Bounds = bounds;
            data.ForceBoudaryStopAtConnection();

            Catalog.SaveToJson(data);
        }

        [Button]
        public void UpdateDataConnectionPositionAndDirection()
        {
            Catalog.EditorLoadAllJson();
            AreaData data = areaData.Data;
            if (data == null) return;

            if (data.connections == null)
            {
                data.connections = new List<AreaData.AreaConnection>();
            }

            List<AreaData.AreaConnection> connectionList = new List<AreaData.AreaConnection>();
            AreaGateway[] gateways = GetComponentsInChildren<AreaGateway>();
            for (int i = 0; i < gateways.Length; i++)
            {
                AreaData.AreaConnection connection = new AreaData.AreaConnection();
                connectionList.Add(connection);

                connection.connectionTypeIdContainerList = new List<AreaData.AreaConnectionTypeIdContainer>();
                connection.position = gateways[i].transform.position;
                AreaRotationHelper.Face face;
                if (AreaRotationHelper.TryGetFaceFromQuaterion(gateways[i].transform.rotation, out face))
                {
                    connection.face = face;
                }
                else
                {
                    Debug.LogError($"Cannot find a proper Face for gateway index : {i}");
                }
            }

            int count = Math.Min(data.connections.Count, connectionList.Count);
            for (int i = 0; i < count; i++)
            {
                connectionList[i].connectionTypeIdContainerList = data.connections[i].connectionTypeIdContainerList;
                if (connectionList[i].connectionTypeIdContainerList == null)
                {
                    connectionList[i].connectionTypeIdContainerList = new List<AreaData.AreaConnectionTypeIdContainer>();
                }

                connectionList[i].overrideBlockerTableAdress = data.connections[i].overrideBlockerTableAdress;
            }

            data.connections = connectionList;

            Catalog.SaveToJson(data);
        }

 //ProjectCore

        private void OnDrawGizmos()
        {
            if (spawnableArea != null)
            {
                Gizmos.color = Color.magenta;
                Bounds bounds = spawnableArea.Bounds;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
            else if (Application.isPlaying == false && areaData.Data != null)
            {
                Gizmos.matrix = this.transform.localToWorldMatrix;
                Gizmos.color = Color.magenta;
                Bounds bounds = areaData.Data.Bounds;
                Gizmos.DrawWireCube(bounds.center, bounds.size);

            }
        }


        public void AddLoopedAudioSources()
        {
            foreach (AudioSource audioSource in this.GetComponentsInChildren<AudioSource>())
            {
                if (audioSource.loop)
                {
                    if (!audioSourcesToBlend.Contains(audioSource))
                        audioSourcesToBlend.Add(audioSource);
                }
            }
            UnityEditor.EditorUtility.SetDirty(this.gameObject);
        }

        public List<Issue> GetIssues(bool includeLongCheck, bool experimental)
        {
            return default;
 //ProjectCore
        }


 //ProjectCore

        /// <summary>
        /// Check if gateway box detection intersect each other
        /// Note: the player shouldn't be in 2 gateway box at the same time
        /// </summary>
        public void CheckGatewayIntersect()
        {
            for (int i = 0; i < gateways.Length; i++)
            {
                var bounds = gateways[i].GetWorldBounds();
                for (int j = 0; j < gateways.Length; j++)
                {
                    if (i == j) continue;

                    if(bounds.Intersects(gateways[j].GetWorldBounds()))
                    {
                        Debug.LogError("Area : " + gameObject.name + " gateway " + i + "/" + j + " intersect each other");
                    }
                }
            }

        }
#endif //UNITY_EDITOR
        #endregion Editor
    }
}