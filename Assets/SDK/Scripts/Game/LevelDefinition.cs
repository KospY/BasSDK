using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class LevelDefinition : MonoBehaviour
    {
        public static LevelDefinition current;
        public Transform playerStart;
        public List<CustomReference> customReferences;
        public bool loadDefaultCharIfNeeded = true;

        [NonSerialized]
        public AudioSource music;
        [NonSerialized]
        public bool initialized;

        [Serializable]
        public class CustomReference
        {
            public string name;
            public List<Transform> transforms;
        }

        [Button]
        public static void CheckLightMapMode()
        {
            Debug.Log("Lightmap mode: " + LightmapSettings.lightmapsMode);
        }

#if ProjectCore

        [NonSerialized, ShowInInspector, ReadOnly]
        public LevelData data;
        [NonSerialized, ShowInInspector, ReadOnly]
        public LevelData.ModeRank modeRank;

        protected virtual void Awake()
        {
            if (gameObject.scene.name.ToLower() != "master")
            {
                current = this;
            }

            if (loadDefaultCharIfNeeded && GameManager.playerData == null)
            {
                Debug.Log("No player data loaded, get first character slot");
                List<CharacterData> characterDataList = DataManager.GetCharacters();
                if (GameManager.local.defaultCharacterIndex < characterDataList.Count && GameManager.local.defaultCharacterIndex >= 0)
                {
                    GameManager.playerData = characterDataList[GameManager.local.defaultCharacterIndex];
                }
                else if (characterDataList.Count > 0)
                {
                    GameManager.playerData = characterDataList[0];
                }
                else
                {
                    Debug.LogError("No character found in saves");
                    GameManager.playerData = new CharacterData("Temp", "Temp");
                }
            }
        }

        protected virtual void Start()
        {
            StartCoroutine(OnLevelStartedCoroutine());
        }

        protected virtual void Update()
        {
            if (initialized)
            {
                foreach (LevelModule levelModeModule in modeRank.mode.modules)
                {
                    levelModeModule.Update(this);
                }
            }
        }

        protected virtual IEnumerator OnLevelStartedCoroutine()
        {
            VolumetricFogAndMist2.VolumetricFogManager volumetricFogManager = GameObject.FindObjectOfType<VolumetricFogAndMist2.VolumetricFogManager>();
            if (volumetricFogManager)
            {
                volumetricFogManager.gameObject.SetActive(false);
            }

            // Wait game to initalize
            while (!GameManager.initialized) yield return new WaitForEndOfFrame();

            if (this.gameObject.scene.name.ToLower() == "master")
            {
                data = Catalog.GetData<LevelData>("Master");
            }
            else
            {
                while (SceneManager.GetActiveScene() != this.gameObject.scene) yield return new WaitForEndOfFrame();
            }

            try
            {
                if (modeRank == null) modeRank = data.GetFirstModRank();
                // Load level mode module
                foreach (LevelModule levelModeModule in modeRank.mode.modules)
                {
                    levelModeModule.OnLevelLoaded(this);
                }
            }
            catch (Exception e)
            {
                LoadingCamera.SetState(LoadingCamera.State.Error);
                throw;
            }

            // Wait modules to load
            foreach (LevelModule levelModeModule in modeRank.mode.modules)
            {
                while (!levelModeModule.initialized) yield return new WaitForEndOfFrame();
            }

            if (data.id.ToLower() != "master")
            {
                Player player = null;
                try
                {
                    // Spawn player
                    if (data.spawnPlayer)
                    {
                        player = Instantiate(Resources.Load("Player", typeof(Player)) as Player, playerStart.position, playerStart.rotation);
                        player.morphology = GameManager.playerData.morphology;
                        EventManager.InvokePlayerSpawned(player);
                    }

                    if (player && Application.platform != RuntimePlatform.Android)
                    {
                        GameManager.liv.HMDCamera = player.head.cam;
                        GameManager.liv.TrackedSpaceOrigin = player.transform;
                        GameManager.liv.enabled = true;
                    }

                }
                catch (Exception e)
                {
                    LoadingCamera.SetState(LoadingCamera.State.Error);
                    throw;
                }

                // Spawn player body
                if (data.spawnBody && player)
                {
                    Creature playerCreature = null;
                    try
                    {
                        playerCreature = Catalog.GetData<CreatureData>(GameManager.playerData.GetCreatureID()).Spawn(playerStart.position, playerStart.rotation, false);
                        playerCreature.container.containerID = null;
                        playerCreature.container.content = GameManager.playerData.inventory;
                    }
                    catch (Exception e)
                    {
                        LoadingCamera.SetState(LoadingCamera.State.Error);
                        throw;
                    }

                    while (!playerCreature.initialized) yield return new WaitForEndOfFrame();

                    if (playerCreature.umaCharacter)
                    {
                        try
                        {
                            playerCreature.umaCharacter.LoadUmaPreset(GameManager.playerData.umaPreset, null, playerCreature.container);
                        }
                        catch (Exception e)
                        {
                            LoadingCamera.SetState(LoadingCamera.State.Error);
                            throw;
                        }
                        while (playerCreature.umaCharacter.characterDataLoading) yield return new WaitForEndOfFrame();
                    }
                    while (!playerCreature.initialized) yield return new WaitForEndOfFrame();
                    try
                    {
                        player.SetBody(playerCreature.body);
                    }
                    catch (Exception e)
                    {
                        LoadingCamera.SetState(LoadingCamera.State.Error);
                        throw;
                    }
                }
                try
                {
                    if (data.fadeOutTime > 0) PostProcessManager.local.DoTimedEffect(Color.black, PostProcessManager.TimedEffect.FadeOut, data.fadeOutTime);
                }
                catch (Exception e)
                {
                    LoadingCamera.SetState(LoadingCamera.State.Error);
                    throw;
                }
            }

            if (data.musicAudioContainer)
            {
                music = this.gameObject.AddComponent<AudioSource>();
                music.loop = true;
                music.outputAudioMixerGroup = GameManager.GetAudioMixerGroup(AudioMixerName.Music);
                music.clip = data.musicAudioContainer.PickAudioClip();
                music.Play();
                if (data.musicAudioContainer.sounds.Count > 1)
                {
                    Invoke("MusicChange", music.clip.length);
                }
            }
            
            if (volumetricFogManager)
            {
                volumetricFogManager.gameObject.SetActive(true);
            }

            LoadingCamera.SetState(LoadingCamera.State.Disabled);
            initialized = true;
        }

        public virtual void MusicChange()
        {
            music.clip = data.musicAudioContainer.PickAudioClip();
        }

        public virtual void OnLevelUnload()
        {
            foreach (LevelModule levelModeModule in modeRank.mode.modules)
            {
                levelModeModule.OnLevelUnloaded(this);
            }
        }
#endif
    }
}
