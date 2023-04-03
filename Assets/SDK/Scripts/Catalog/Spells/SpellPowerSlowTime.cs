using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class SpellPowerSlowTime : SpellPowerData
    {
#if ODIN_INSPECTOR
        [BoxGroup("SlowTime"), ValueDropdown("GetAllEffectID")] 
#endif
        public string effectId;
        [NonSerialized]
        public EffectData effectData;
#if ODIN_INSPECTOR
        [BoxGroup("SlowTime")] 
#endif
        public bool scaleToMixerParameter;
#if ODIN_INSPECTOR
        [BoxGroup("SlowTime")] 
#endif
        public string scaleToMixerParameterName;
#if ODIN_INSPECTOR
        [BoxGroup("SlowTime")] 
#endif
        public float scale = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("SlowTime")] 
#endif
        public AnimationCurve enterCurve;
#if ODIN_INSPECTOR
        [BoxGroup("SlowTime")] 
#endif
        public AnimationCurve exitCurve;

#if ODIN_INSPECTOR
        [BoxGroup("Focus")] 
#endif
        public float focusConsumption = 5;
#if ODIN_INSPECTOR
        [BoxGroup("Focus")] 
#endif
        public float focusMinToUse = 10;

#if ODIN_INSPECTOR
        [BoxGroup("Focus"), ValueDropdown("GetAllEffectID")] 
#endif
        public string focusDepletedEffectId;
        [NonSerialized]
        public EffectData focusDepletedEffetData;

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            if (effectId != null && effectId != "") effectData = Catalog.GetData<EffectData>(effectId);
            if (focusDepletedEffectId != null && focusDepletedEffectId != "") focusDepletedEffetData = Catalog.GetData<EffectData>(focusDepletedEffectId);
        }

    }
}
