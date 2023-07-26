using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class LevelModuleSurvival : LevelModule
    {

        #region Classes

        [Serializable]
        public class Waves
        {
#if ODIN_INSPECTOR
            [ValueDropdown("GetAllWaveID")]
#endif
            public string waveID;
#if ODIN_INSPECTOR
            [ValueDropdown("GetAllContainerID")]
#endif
            public string containerID;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllWaveID()
            {
                return Catalog.GetDropdownAllID(Category.Wave);
            }

            public List<ValueDropdownItem<string>> GetAllContainerID()
            {
                return Catalog.GetDropdownAllID(Category.Container);
            }
#endif
        }

        #endregion

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllTextGroupID")]
#endif
        public string textGroupId;
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllTextId")]
#endif
        public string textNextWaveId;
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllTextId")]
#endif
        public string textFightId;
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllTextId")]
#endif
        public string textWaveId;

        public float startDelay = 10;
        public int delayBetweenWave = 10;
        public bool disallowHealthPotions = true;
        public bool repeatLastWaveIndefinitely = true;
        public bool canOnlyUseRewards = false;
        public int rewardsToSpawn = 3;
        protected List<Transform> rewardsSpawnPosition;
        protected List<Animator> rewardsAnimator;
        protected List<AudioSource> rewardsSound;
        protected List<GameObject> rewardsFX;

        public float drivePositionSpring = 100;
        public float drivePositionDamper = 2;
        public float slerpPositionSpring = 30;
        public float slerpPositionDamper = 0;
        public float spawnPositionHeight = 2;

        public int wavesNumberForReward = 1;
        
        public List<Waves> waves;
        
        protected int currentWaveNumberForReward;

        protected int waveIndex;

        protected LevelModuleTutorial levelModuleTutorial;
        protected WaveSpawner waveSpawner;

        protected bool waitingToChooseReward;
        protected List<Item> rewards;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllContainerID")]
#endif
        public string firstRewardsContainerID;

        #region ODIN_METHODS

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllTextGroupID()
        {
            return Catalog.GetTextData().GetDropdownAllTextGroups();
        }

        public List<ValueDropdownItem<string>> GetAllTextId()
        {
            return Catalog.GetTextData().GetDropdownAllTexts(textGroupId);
        }

        public List<ValueDropdownItem<string>> GetAllContainerID()
        {
            return Catalog.GetDropdownAllID(Category.Container);
        }

        public List<ValueDropdownItem<string>> GetAllWaveID()
        {
            return Catalog.GetDropdownAllID(Category.Wave);
        }
#endif

        #endregion

    }
}
