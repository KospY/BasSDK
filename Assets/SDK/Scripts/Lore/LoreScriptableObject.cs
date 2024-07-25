using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif // ODIN_INSPECTOR

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif //UNITY_EDITOR


namespace ThunderRoad
{
    [CreateAssetMenu(menuName = "ThunderRoad/Lore/Lore")]
    public class LoreScriptableObject : ScriptableObject
    {
        public enum LoreType
        {
            text,
            texture
        }

        [Serializable]
        public class LoreData
        {
            public string groupId;
            public string titleId;
            public string loreId;
            public string itemId;
            public LoreType loreType;
            public string contentAddress;
            public bool displayGraphicsInJournal;
        }

        [Serializable]
        public class LorePack
        {
            public int hashId;

            [Header("Group name translate")]
            public string groupId;
            public string nameId;

            [Header("LoreRequirement")]
            public List<int> loreRequirement;

            [Header("Conditions")]
            public LorePackCondition lorePackCondition;

            public bool spawnPackAsOneItem;

            [Header("Lore Data")]
            public List<LoreData> loreData;
#if UNITY_EDITOR
            [NonSerialized]
            public int journalIndex;
#endif //UNITY_EDITOR

        }

        public LoreDisplayTypeScriptableObject displayType;

#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public LorePack[] allLorePacks;
#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public List<int> rootLoreHashIds = null;
#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public int loreId;
#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public int changeCheckHash;
#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public HashSet<string> uniqueRequiredParamsInPack = new HashSet<string>();
#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public HashSet<string> uniqueLevelOptionsInPack = new HashSet<string>();

        private Dictionary<int, LorePack> _hashIdToLorePack;

        public LorePack GetPack(int hashId)
        {
            if (_hashIdToLorePack == null)
            {
                _hashIdToLorePack = new Dictionary<int, LorePack>();
                for (int i = 0; i < allLorePacks.Length; i++)
                {
                    _hashIdToLorePack.Add(allLorePacks[i].hashId, allLorePacks[i]);
                }
            }

            if (_hashIdToLorePack.TryGetValue(hashId, out LorePack value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// Get the lore hash id of a specific lore pack from the lore tree 
        /// </summary>
        /// <param name="lorePackName">Lore pack name must be composed by the lore pack folder name</param>
        /// <returns></returns>
        public static int GetLoreHashId(string lorePackName)
        {
            return Animator.StringToHash(lorePackName);
        }

#if UNITY_EDITOR
        [Button]
        public void CreateLoreTree()
        {
            string fullPath = AssetDatabase.GetAssetPath(this);
            string directoryPath = Path.GetDirectoryName(fullPath);

            _hashIdToLorePack = null;
            List<LorePackScriptableObject> allPack = GetAllLorePack(directoryPath);
            Dictionary<int, LorePack> allLorePacksDictionary = new Dictionary<int, LorePack>();

            HashSet<int> validHashId = new HashSet<int>();
            HashSet<int> topHashId = new HashSet<int>();


            bool hasChanged = true;
            int packCount = allPack.Count;
            while (packCount > 0 && hasChanged)
            {
                hasChanged = false;

                for (int indexPack = packCount - 1; indexPack >= 0; indexPack--)
                {
                    LorePackScriptableObject currentPack = allPack[indexPack];
                    List<LorePackScriptableObject> requirements = currentPack.loreRequirements;
                    int requirementCount = requirements != null ? requirements.Count : 0;
                    List<int> tempValidHashId = new List<int>();
                    for (int i = 0; i < requirementCount; i++)
                    {
                        int tempHash = GetLorePackSoHashId(requirements[i]);
                        if (validHashId.Contains(tempHash))
                        {
                            tempValidHashId.Add(tempHash);
                        }
                    }

                    // All requirements are valid. Create node and add it as valid
                    if (tempValidHashId.Count == requirementCount)
                    {
                        LorePack lorePack = GetLorePackFrom(currentPack);
                        lorePack.loreRequirement = tempValidHashId;
                        allLorePacksDictionary.TryAdd(lorePack.hashId, lorePack);
                        validHashId.Add(lorePack.hashId);
                        hasChanged = true;

                        topHashId.Add(lorePack.hashId);
                        for (int i = 0; i < requirementCount; i++)
                        {
                            int key = tempValidHashId[i];
                            if (topHashId.Contains(key))
                            {
                                topHashId.Remove(key);
                            }
                        }

                        allPack.RemoveAt(indexPack);
                    }
                }

                packCount = allPack.Count;
            }

            rootLoreHashIds = new List<int>();
            foreach (var item in topHashId)
            {
                rootLoreHashIds.Add(item);
            }

            List<LorePack> lorePackList = new List<LorePack>();
            foreach (KeyValuePair<int, LorePack> item in allLorePacksDictionary)
            {
                int count = lorePackList.Count;
                int index = -1;
                for (int i = 0; i < count; i++)
                {
                    if (lorePackList[i].journalIndex > item.Value.journalIndex)
                    {
                        index = i;
                        break;
                    }
                }

                if (index < 0)
                {
                    lorePackList.Add(item.Value);
                }
                else
                {
                    lorePackList.Insert(index, item.Value);
                }
            }


            allLorePacks = lorePackList.ToArray();

            changeCheckHash = GetTreeHashId(rootLoreHashIds);

            string tempPath = fullPath.Replace(Path.DirectorySeparatorChar, '/'); // be sure to always use '/' as separator
            loreId = Animator.StringToHash(tempPath);

            for (int i = 0; i < packCount; i++)
            {
                Debug.LogError("Lore Pack : " + allPack[i].groupId + "/" + allPack[i].nameId + " can not be resolve. Maybe requirement are looping ?");
            }
        }

        private List<LorePackScriptableObject> GetAllLorePack(string directoryPath)
        {
            List<LorePackScriptableObject> list = new List<LorePackScriptableObject>();

            string[] folder = new string[1];
            folder[0] = directoryPath;
            string[] results = AssetDatabase.FindAssets("t:LorePackScriptableObject", folder);
            for (int i = 0; i < results.Length; i++)
            {
                string configPath = AssetDatabase.GUIDToAssetPath(results[i]);
                LorePackScriptableObject pack = AssetDatabase.LoadAssetAtPath<LorePackScriptableObject>(configPath);
                if (pack != null)
                {
                    list.Add(pack);
                }
            }

            return list;
        }

        public LorePack GetLorePackFrom(LorePackScriptableObject packSo)
        {
            LorePack lorePack = new LorePack();

            lorePack.groupId = packSo.groupId;
            lorePack.nameId = packSo.nameId;
            lorePack.journalIndex = packSo.journalIndex;


            lorePack.hashId = GetLorePackSoHashId(packSo);


            lorePack.lorePackCondition = new LorePackCondition(packSo.condition);
            lorePack.spawnPackAsOneItem = packSo.spawnPackAsSingleItem;
            uniqueRequiredParamsInPack.UnionWith(packSo.condition.requiredParameters);

            foreach(LorePackCondition.LoreLevelOptionCondition condition in packSo.condition.levelOptions)
            {
                uniqueLevelOptionsInPack.Add($"{condition.key} : {condition.value}");
            }

            lorePack.loreData = new List<LoreData>();
            for (int i = 0; i < packSo.loreData.Count; i++)
            {
                LoreData temp = new LoreData()
                {
                    groupId = packSo.loreData[i].groupId,
                    titleId = packSo.loreData[i].titleId,
                    loreId = packSo.loreData[i].loreId,
                    itemId = packSo.itemId,
                    loreType = packSo.loreData[i].loreType,
                    contentAddress = packSo.loreData[i].contentType,
                    displayGraphicsInJournal = packSo.loreData[i].displayGraphicsInJournal,
                };

                lorePack.loreData.Add(temp);
            }

            return lorePack;
        }

        private int GetLorePackSoHashId(LorePackScriptableObject pack)
        {
            return GetLoreHashId(pack.nameId);
        }

        private int GetTreeHashId(int nodeHashId)
        {
            string nodeId = nodeHashId.ToString();
            LorePack lorePack = GetPack(nodeHashId);
            if (lorePack.loreRequirement != null)
            {
                int count = lorePack.loreRequirement.Count;
                for (int i = 0; i < count; i++)
                {
                    int hash = GetTreeHashId(lorePack.loreRequirement[i]);
                    nodeId += hash.ToString();
                }
            }

            return Animator.StringToHash(nodeId);
        }

        private int GetTreeHashId(List<int> rootHashIds)
        {
            string nodeId = string.Empty;
            int count = rootHashIds.Count;
            for (int i = 0; i < count; i++)
            {
                int hash = GetTreeHashId(rootHashIds[i]);
                nodeId += hash.ToString();
            }

            return Animator.StringToHash(nodeId);
        }

        [MenuItem("ThunderRoad (SDK)/Lore/CreateAllLoreTree")]
        public static void CreateAllLoreTree()
        {
            string[] guids = AssetDatabase.FindAssets("t:"+typeof(LoreScriptableObject).Name);

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                LoreScriptableObject lore = AssetDatabase.LoadAssetAtPath<LoreScriptableObject>(path);
                lore.CreateLoreTree();
            }
        }

        [Serializable]
        public struct LoreOrderTitles
        {
#if ODIN_INSPECTOR
            [ReadOnly]
#endif
            public string packTitle;
#if ODIN_INSPECTOR
            [ReadOnly]
#endif
            public List<string> noteTitles;

#if ODIN_INSPECTOR
            [FoldoutGroup("pack")]
#endif // ODIN_INSPECTOR
            public LorePackScriptableObject lorePack;

            public LoreOrderTitles(LorePackScriptableObject lorePack)
            {
                this.lorePack = lorePack;
                packTitle = lorePack.nameId;

                noteTitles = new List<string>();
                for (int i = 0; i < lorePack.loreData.Count; i++)
                {
                    noteTitles.Add(lorePack.loreData[i].titleId);
                }
            }
        }

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public List<LoreOrderTitles> loreOrder;
        [Button]
        public void LoadLoreOrder()
        {
            string fullPath = AssetDatabase.GetAssetPath(this);
            string directoryPath = Path.GetDirectoryName(fullPath);
            List<LorePackScriptableObject> allPack = GetAllLorePack(directoryPath);

            loreOrder = new List<LoreOrderTitles>();
            int allPackCount = allPack.Count;
            for (int i = 0; i < allPackCount; i++)
            {
                LorePackScriptableObject tempPack = allPack[i];
                int loreOrderCount = loreOrder.Count;
                int index = -1;
                for (int j = 0; j < loreOrderCount; j++)
                {
                    if (loreOrder[j].lorePack.journalIndex > tempPack.journalIndex)
                    {
                        index = j;
                        break;
                    }
                }

                if (index < 0)
                {
                    loreOrder.Add(new LoreOrderTitles(tempPack));
                }
                else
                {
                    loreOrder.Insert(index, new LoreOrderTitles(tempPack));
                }
            }
        }

        [Button]
        public void SetLoreOrder()
        {
            if (loreOrder == null)
            {
                return;
            }

            int count = loreOrder.Count;
            for (int i = 0; i < count; i++)
            {
                var tempPack = loreOrder[i].lorePack;
                tempPack.journalIndex = i;
                EditorUtility.SetDirty(tempPack);
            }

            AssetDatabase.SaveAssets();

            CreateLoreTree();
        }
#endif //UNITY_EDITOR
    }
}