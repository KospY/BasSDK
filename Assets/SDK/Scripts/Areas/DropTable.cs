using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
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


        /// <summary>
        /// Calculates the percentage and asigns the probabilities how many times
        /// the items can be picked. Function used also to validate data when tweaking numbers in editor.
        /// </summary>	
        public void CalculateWeight()
        {
        }

        private static float ComputeDropListRange(ref List<Drop> dropList)
        {
return 0;
        }

        #endregion Methods
    }
}