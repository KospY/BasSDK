using System;
using System.Collections;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif //ODIN_INSPECTOR
using ThunderRoad.Modules;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThunderRoad
{
    public class LoreSpawner : MonoBehaviour
    {
        #region Public Fields 
        public LorePackCondition.Visibility visibility;
        public List<string> loreConditionRequiredParameters;
        public List<string> loreConditionOptionalParameters;
        public bool isGroupLoreSpawn = false;
#if ODIN_INSPECTOR
        [ShowIf("isGroupLoreSpawn")]
#endif
        public List<Transform> spawnPoints = new List<Transform>();
        public List<ILoreDisplay> loreDisplayers = new List<ILoreDisplay>();
        public List<Item> loreItems = new List<Item>();
        public bool IsPopulated => isPopulated;
        public bool ForceEnable = false;
        public bool SpawnOnStart = true;
        public bool ManualCulling = false;
#if ODIN_INSPECTOR
        [ShowIf("ManualCulling")]
#endif
        public float CullingDistance = 50f;
        public bool useSpecificLorePack;
#if ODIN_INSPECTOR
        [ShowIf("useSpecificLorePack")]
#endif
        [Tooltip("The lore pack name starts with the local path of the pack folder name")]
        public string specificLorePackName;

        public ItemSpawner parentSpawner;


#if ODIN_INSPECTOR
        [ShowIf("DEBUG")]
#endif
        public LoreArea loreArea;
        #endregion

        #region Private 
        private LoreModule loreModule = null;
        private int lorePackId = -1;
        private bool hasRead = false;
        private bool isPopulated;
        private List<LoreScriptableObject.LoreData> loreData;
        private LoreScriptableObject.LorePack lorePack;
        private EffectData loreFoundEffectData;
        private string LoreFoundSfxID = "LoreFound";
        private bool initialized = false;
        #endregion


        public void Init()
        {
        }


        public void PopulateLore()
        {
        }


        public void MarkAsRead(bool playEffect)
        {
        }

    }
}
