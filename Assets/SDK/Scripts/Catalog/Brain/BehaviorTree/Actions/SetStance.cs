
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Action
{
	public class SetStance : ActionNode
    {
#if ODIN_INSPECTOR
        [OnValueChanged("CheckStanceSubStanceValid")]
#endif
        public BrainModuleStance.Stance stance = BrainModuleStance.Stance.Idle;
        public bool onlyOnce = false;

        public enum ChangeOptions
        {
            Random,
            Specific,
        }
#if ODIN_INSPECTOR
        [HideIf("@this.stance == BrainModuleStance.Stance.Idle || this.stance == BrainModuleStance.Stance.Bow || this.stance == BrainModuleStance.Stance.Flee"), OnValueChanged("CheckStanceSubStanceValid")]
#endif
        public ChangeOptions setSubStance = ChangeOptions.Random;
#if ODIN_INSPECTOR
        [HideIf("@this.stance == BrainModuleStance.Stance.Idle || this.stance == BrainModuleStance.Stance.Bow || this.stance == BrainModuleStance.Stance.Flee"), ShowIf("setSubStance", optionalValue: ChangeOptions.Random)]
#endif
        public bool excludeCurrent = true;
#if ODIN_INSPECTOR
        [HideIf("@this.stance == BrainModuleStance.Stance.Idle || this.stance == BrainModuleStance.Stance.Bow || this.stance == BrainModuleStance.Stance.Flee"), ShowIf("setSubStance", optionalValue: ChangeOptions.Specific)]
#endif
        public string subStanceName = "";

#if ODIN_INSPECTOR
        private void CheckStanceSubStanceValid()
        {
            if (stance == BrainModuleStance.Stance.Idle || stance == BrainModuleStance.Stance.Bow || stance == BrainModuleStance.Stance.Flee)
            {
                if (setSubStance != ChangeOptions.Random) setSubStance = ChangeOptions.Random;
            }
        }
#endif

    }
}
