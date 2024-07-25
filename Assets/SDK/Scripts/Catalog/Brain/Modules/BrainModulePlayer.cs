using UnityEngine;
using System;
using System.Collections.Generic;
using static ThunderRoad.Brain;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BrainModulePlayer : BrainData.Module
    {
        #region Enum
        public enum Exposure
        {
            None,
            Danger,
            Alert,
            RangedCombat,
            CloseCombat,
        }
        #endregion Enum

        #region Fields
        #region Public
        public static bool playerCanLookSpectator = false;
        public static List<Creature> playerModuleIgnoreCreatures = new List<Creature>();

        [Header("Exposure")]
        public float dangerDistanceEntrance = 14;
        public float dangerDistanceExit = 16;

        [Header("Player eyes")]
        public bool playerEyeAnimation = true;
        public float focusAngle = 30f;
        public Vector2 lookDistanceMinMax = new Vector2(0.2f, 1.5f);

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly] 
#endif
        [NonSerialized]
        public Exposure exposure = Exposure.None;
        #endregion Public

        #region Internals
        private bool _isInDangerZone = false;

        private HashSet<Creature> _creatureInAlert = new HashSet<Creature>();
        private HashSet<Creature> _creatureInMeleeCombat = new HashSet<Creature>();
        private HashSet<Creature> _creatureInRangedCombat = new HashSet<Creature>();

        private BrainModuleLookAt moduleLookAt;
        #endregion Internals
        #endregion Fields

    }
}