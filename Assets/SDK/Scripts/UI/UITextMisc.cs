using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
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
