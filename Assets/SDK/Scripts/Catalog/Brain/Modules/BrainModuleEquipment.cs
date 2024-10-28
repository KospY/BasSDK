using System;
using System.Collections;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BrainModuleEquipment : BrainData.Module
    {
        public static float drawDelay = 0.8f;

        public bool allowArmGrabDisarm = true;
        public int grabDisarmPushLevel = 2;

        public bool allowArmStabDisarm = true;

        public int handHitDisarmPushLevel = 2;

#if ODIN_INSPECTOR
        [MinMaxSlider(0, 10, showFields: true)] 
#endif
        public Vector2 dropObjectsDelay = new Vector2(0, 0.5f);

        [NonSerialized]
        public bool isDrawing;
        [NonSerialized]
        public bool isHolstering;
        [NonSerialized]
        public Item rightBeingDrawn;
        [NonSerialized]
        public Item leftBeingDrawn;

        public static int hashDrawLeft, hashDrawRight;
        public static bool hashInitialized;

    }
}