using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace BS
{
    public class LevelDefinition : MonoBehaviour
    {
        public static LevelDefinition current;
        public Transform playerStart;
        public List<CustomReference> customReferences;

        [Serializable]
        public class CustomReference
        {
            public string name;
            public Transform transform;
        }

#if FULLGAME

        public LevelData data;

        protected virtual void Awake()
        {
            // Load default character if no player data
            if (GameManager.playerData == null && GameManager.GetCurrentLevel() != GameManager.levelCharacterSelection.ToLower() && GameManager.GetCurrentLevel() != "master")
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
                GameManager.playerData.UpdateVersion();
            }

            data = Catalog.current.GetData<LevelData>(this.gameObject.scene.name);
            if (data != null)
            {
                if (data.id.ToLower() != "master") current = this;
            }
            else
            {
                Debug.LogError("Data not found for scene: " + this.gameObject.scene.name);
                this.gameObject.SetActive(false);
            }
        }

        protected virtual void Start()
        {
            StartCoroutine(OnLevelStartedCoroutine());
        }

        protected virtual void Update()
        {
            if (data.modules != null)
            {
                foreach (LevelModule levelModule in data.modules)
                {
                    levelModule.Update(this);
                }
            }
        }

        protected virtual IEnumerator OnLevelStartedCoroutine()
        {
            // Wait game to initalize
            while (!GameManager.initialized) yield return new WaitForEndOfFrame();

            // Load module
            if (data.modules != null)
            {
                foreach (LevelModule levelModule in data.modules)
                {
                    levelModule.OnLevelLoaded(this);
                }
            }

            // Wait modules to load
            if (data.modules != null)
            {
                foreach (LevelModule levelModule in data.modules)
                {
                    while (!levelModule.initialized) yield return new WaitForEndOfFrame();
                }
            }

            if (data.id.ToLower() != "master")
            {
                // Spawn player
                Player player = null;
                if (data.spawnPlayer)
                {
                    player = Instantiate(Resources.Load("Player", typeof(Player)) as Player, playerStart.position, playerStart.rotation);
                    player.morphology = GameManager.playerData.morphology;
                    GameManager.local.FirePlayerSpawnedEvent(player);
                    if (data.fadeOutTime > 0) player.camEffect.DoTimedEffect(Color.black, PlayerCamEffect.TimedEffect.FadeOut, data.fadeOutTime, false);
                }

                if (player && Application.platform != RuntimePlatform.Android)
                {
                    GameManager.liv.HMDCamera = player.head.cam;
                    GameManager.liv.TrackedSpaceOrigin = player.transform;
                    GameManager.liv.enabled = true;
                }

                // Spawn player body
                if (data.spawnBody && player)
                {
                    Creature playerCreature = Catalog.current.GetData<CreatureData>(CharacterData.defaultCreatureID).Instantiate(playerStart.position, playerStart.rotation);
                    playerCreature.container.containerID = null;
                    playerCreature.loadUmaPreset = false;
                    playerCreature.container.content = GameManager.playerData.inventory;
                    if (playerCreature.umaCharacter)
                    {
                        playerCreature.umaCharacter.LoadUmaPreset(GameManager.playerData.umaPreset, null);
                        while (playerCreature.umaCharacter.characterDataLoading) yield return new WaitForEndOfFrame();
                    }
                    while (!playerCreature.initialized) yield return new WaitForEndOfFrame();
                    player.SetBody(playerCreature.body);
                }

                GameManager.SetLoadingCamera(false);
            }
        }


        public virtual void OnLevelUnload()
        {
            if (data.modules != null)
            {
                foreach (LevelModule levelModule in data.modules)
                {
                    levelModule.OnLevelUnloaded(this);
                }
            }
        }

        public virtual void OnCreatureDeath(Creature creature, bool wasPlayer)
        {
            if (data.modules != null)
            {
                foreach (LevelModule levelModule in data.modules)
                {
                    levelModule.OnCreatureDeath(this, creature, wasPlayer);
                }
            }
        }
#endif
    }
}
