using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class DropTable<T>
    {
        #region Data
#if ODIN_INSPECTOR
        [TableList(AlwaysExpanded = true)] 
#endif
        public List<Drop> drops;
        private float probabilityTotalWeight;
        //private static int maxPickCount = 20;
        #endregion Data

        [Serializable]
        public class Drop
        {
#if ODIN_INSPECTOR
            [PropertyOrder(0)] 
#endif
            [JsonMergeKey]
            public T dropItem;

#if ODIN_INSPECTOR
            [PropertyOrder(1)] 
#endif
            public float probabilityWeight;

#if ODIN_INSPECTOR
            [PropertyOrder(2)]
            [ShowInInspector] 
            [ReadOnly]
            [ProgressBar(0, 100)]
#endif
            [NonSerialized]
            public float probabilityPercent;

            [NonSerialized]
            public float probabilityRangeFrom;
            [NonSerialized]
            public float probabilityRangeTo;

            public Drop Clone()
            {
                return MemberwiseClone() as Drop;
            }
        }


        #region Methods
        public DropTable()
        {
            drops = new List<Drop>();
        }

        public void AddDrop(T item, float weight)
        {
            Drop drop = new Drop();
            drop.dropItem = item;
            drop.probabilityWeight = weight;
            drops.Add(drop);
        }

        /// <summary>
        /// Calculates the percentage and asigns the probabilities how many times
        /// the items can be picked. Function used also to validate data when tweaking numbers in editor.
        /// </summary>	
        public void CalculateWeight()
        {
            if (drops == null || drops.Count == 0) return;

            probabilityTotalWeight = ComputeDropListRange(ref drops);
            // Calculate percentage of item drop select rate.
            foreach (Drop lootDropItem in drops)
            {
                lootDropItem.probabilityPercent = ((lootDropItem.probabilityWeight) / probabilityTotalWeight) * 100;
            }
        }

        private static float ComputeDropListRange(ref List<Drop> dropList)
        {
            if (dropList == null || dropList.Count == 0) return 0;

            float currentProbabilityWeightMaximum = 0f;
            // Sets the weight ranges of the selected items.
            foreach (Drop line in dropList)
            {
                if (line.probabilityWeight < 0f)
                {
                    // Prevent usage of negative weight.
                    line.probabilityWeight = 0f;
                }
                else
                {
                    line.probabilityRangeFrom = currentProbabilityWeightMaximum;
                    currentProbabilityWeightMaximum += line.probabilityWeight;
                    line.probabilityRangeTo = currentProbabilityWeightMaximum;
                }
            }
            return currentProbabilityWeightMaximum;
        }

        // Can return null
        public bool TryPick(out T item, System.Random randomGen = null)
        {
            CalculateWeight();

            float pickedNumber = randomGen != null ? (float) randomGen.NextDouble() * probabilityTotalWeight
                                                    : UnityEngine.Random.Range(0, probabilityTotalWeight);

            // Find an item whose range contains pickedNumber
            int count = drops.Count;
            for (int i = 0; i < count; i++)
            {
                Drop drop = drops[i];
                // If the picked number matches the item's range, return item
                if (pickedNumber > drop.probabilityRangeFrom && pickedNumber < drop.probabilityRangeTo)
                {
                    item = drop.dropItem;
                    return true;
                }
            }

            item = default(T);
            return false;
        }

        public List<T> PickOrder(out List<int> indexOrder)
        {
            indexOrder = null;
            if (drops == null || drops.Count == 0) return null;

            indexOrder = new List<int>();
            int dropCount = drops.Count;
            List<T> order = new List<T>();
            List<Drop> dropList = new List<Drop>(dropCount);
            List<int> tempIndexOrder = new List<int>();
            for (int i = 0; i < dropCount; i++)
            {
                dropList.Add(drops[i].Clone());
                tempIndexOrder.Add(i);
            }

            while(dropList.Count > 0)
            {
                float totalWeight = ComputeDropListRange(ref dropList);
                if (totalWeight == 0) break;

                float pickedNumber = UnityEngine.Random.Range(0, totalWeight);

                // Find an item whose range contains pickedNumber
                int count = dropList.Count;
                for (int i = 0; i < count; i++)
                {
                    Drop drop = dropList[i];
                    int index = tempIndexOrder[i];
                    // If the picked number matches the item's range, return item
                    if (pickedNumber > drop.probabilityRangeFrom && pickedNumber < drop.probabilityRangeTo)
                    {
                        tempIndexOrder.RemoveAt(i);
                        dropList.RemoveAt(i);
                        indexOrder.Add(index);
                        order.Add(drop.dropItem);
                        break;
                    }
                }
            }

            return order;
        }

        [Button]
        public void RefreshProbability()
        {
            CalculateWeight();
        }
        #endregion Methods
    }
}