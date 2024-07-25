using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class FirearmStanceData : StanceData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Stance basics"), OnValueChanged(nameof(UpdateIDs))]
#endif
        public string rootName;
#if ODIN_INSPECTOR
        [BoxGroup("Shooting"), HideLabel, Header("Idle")]
#endif
        public FirearmPose idlePose = new FirearmPose();
#if ODIN_INSPECTOR
        [BoxGroup("Shooting"), HideLabel, Header("Aim")]
#endif
        public FirearmPose aimPose = new FirearmPose();
#if ODIN_INSPECTOR
        [BoxGroup("Shooting"), HideLabel, Header("Fire")]
#endif
        public FirearmAction fireAction = new FirearmAction();
#if ODIN_INSPECTOR
        [BoxGroup("Shooting"), HideLabel, Header("Reload")]
#endif
        public FirearmAction reloadAction = new FirearmAction();

#if ODIN_INSPECTOR
        protected void UpdateIDs()
        {
            idlePose.id = rootName + "IdlePose";
            aimPose.id = rootName + "AimPose";
            fireAction.id = rootName + "ShootAnimation";
            reloadAction.id = rootName + "ReloadAnimation";
        }
#endif

    }
}
