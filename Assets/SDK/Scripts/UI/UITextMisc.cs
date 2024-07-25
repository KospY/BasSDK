using UnityEngine;
using TMPro;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class UITextMisc : UIText
    {
        public Value value;

        public enum Value
        {
            GameVersion,
            DungeonInfo,
        }

    }
}
