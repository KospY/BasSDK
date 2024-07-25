using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
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
            [ValueDropdown(nameof(GetAllWaveID))]
#endif
            public string waveID;
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllContainerID))]
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

        public enum SurvivalModeLoopBehavior
        {
            NoLoop,
            Loop,
            RepeatLastWave
        }
        #endregion

        // Options
        public bool canOnlyUseRewards = false;

        // Reward
        public string rewardPillarAddress;
        public Bounds? pillarZone;
        public int rewardsToSpawn = 3;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllContainerID))]
#endif
        public string firstRewardsContainerID;

        // Wave
        public float startDelay = 10;
        public SurvivalModeLoopBehavior loop = SurvivalModeLoopBehavior.Loop;
        public int wavesNumberForReward = 1;
        public List<Waves> waves;

        // Text
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextGroupID))]
#endif
        public string textGroupId;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextId))]
#endif
        public string textNextWaveId;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextId))]
#endif
        public string textFightId;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextId))]
#endif
        public string textWaveId;

        // Private
        protected int currentWaveNumberForReward;

        protected int waveIndex;
        protected int waveDisplay; //for tallying how many waves player has completed when the waveIndex loops back to 0.

        protected WaveSpawner waveSpawner;

        protected ArenaPillar rewardPillarPrefab;
        protected int rewardPillarCount;
        protected List<ArenaPillar> rewardPillars;
        protected int rewardItemSpawned = 0;
        protected bool waitingToChooseReward;


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
