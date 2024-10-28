using System.Collections.Generic;
using UnityEngine;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public abstract class ModOptionData<T> : CustomData
    {
        [System.Serializable]
        public class Option
        {
            public string text;
            public T value;

        }

        public enum InteractionType
        {
            ArrowList, //  left/right arrows with the value in the middle (<- Value ->)
            ButtonList, //button which cycles through the options every time its pressed (Value)
            Slider, // horizontal slider with left/right arrows
        }

#if ODIN_INSPECTOR
        [BoxGroup("Display")]
#endif
        public InteractionType interactionType = InteractionType.ArrowList;
#if ODIN_INSPECTOR
        [BoxGroup("Display")]
#endif
        public string optionName;
#if ODIN_INSPECTOR
        [BoxGroup("Display")]
#endif
        public string optionLocalizationID;
#if ODIN_INSPECTOR
        [BoxGroup("Display")]
#endif
        public string tooltip = "";
#if ODIN_INSPECTOR
        [BoxGroup("Display")]
#endif
        public int defaultOptionIndex = -1;
#if ODIN_INSPECTOR
        [BoxGroup("Display")]
#endif
        public bool storeValueInSave = true;
#if ODIN_INSPECTOR
        [BoxGroup("Category and sorting")]
#endif
        public string category;
#if ODIN_INSPECTOR
        [BoxGroup("Category and sorting")]
#endif
        public int categoryOrder = int.MaxValue;
#if ODIN_INSPECTOR
        [BoxGroup("Category and sorting")]
#endif
        public int optionOrder = int.MaxValue;
        public List<Option> options;

    }
}
