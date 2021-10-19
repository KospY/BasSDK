using UnityEngine;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class UITextInventory : UIText
    {
        public Value value;

        public enum Value
        {
            None,
            DisplayName,
            Description,
            Quantity,
            LevelRequired,
            Price,
            Slot,
            Category,
            Author,
            Mass,
            DamagerRecoil,
            DamagerPenetration,
            DamagerPressure,
            DamagerKnockout,
            DamagerSlashAngle,
            DamagerDismemberement,
            QuiverCapacity,
            PotionEffect,
            PotionDuration,
            BowForce,
            Tier,
        }

    }
}
