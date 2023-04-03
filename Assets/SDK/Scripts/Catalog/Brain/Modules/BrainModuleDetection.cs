using UnityEngine;
using System;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class BrainModuleDetection : BrainData.Module
    {
        [Header("Sight")]
        public float sightDetectionVerticalFov = 130;
        public float sightDetectionHorizontalFov = 190;
        public float maxHeightDiff = 10f;
        public bool canSeeLights = true;
        public float sawLightRememberDuration = 30f;
        public float sawTKGrabRememberDuration = 30f;

        [Header("Misc")]
        public float underSpellFireMaxAngle = 50;
        public float acquireSharedTargetDelay = 3f;

        [Header("Hearing")]
        public bool canHear = true;
        public float hearMaxDistance = 10;
        public float hearMinNoiseRatio = 0.5f;
        public float hearRememberDuration = 30f;

        [Header("Sensitivity")]
        // Determines how much an NPC needs to be alerted to notice something. Increasing this value makes it take longer across the board
        public float detectAlertednessThreshold = 10f;
        // The decrease delay defines how long it should take (in seconds) for an NPC's alertedness to start decreasing. This makes it so that NPCs don't immediately start forgetting they detected something
        public float alertednessDecreaseDelay = 1f;
        // Alertedness should decrease over time, it should take up to (almost) 10 seconds for an NPC to be completely "reset" to base
        public float alertednessLossPerSec = 1f;
        // Defines the minimum amount the alertedness can increase by. If the increase is lower than this, the alertedness doesn't increase
        public float minAlertednessIncrease = 0.05f;
        public float enemyInFOVIncrease = 10f;
        public AnimationCurve distanceMultCurve;
        public AnimationCurve angleMultCurve;
        // Multiplies the alertedness increased based on whether or not the creature is crouching. By default this is 1.0, but I think it may make sense for enemies to get alerted faster by crouching enemies.
        public float crouchingMultiplier = 0.6666f;
        // Adjusts how quickly the alertedness bar increases from seeing a dead body
        public float deadBodyMultiplier = 1.5f;
        // The minimum multiplier if the target is in the darkest possible visible brightness
        public float minVisibleMultiplier = 0.5f;
        // Alertedness should increase based on the loudness and distance of sounds: a loud and close sound should make an NPC very alert very quickly, while a far and/or quiet sound shouldn't contribute as much
        public float soundHeardScalableValue = 6.66f;
        // Realtime point lights should increase NPC suspiciousness. This is scaled by brightness
        public float dynamicLightIncreasePerSec = 5f;
        // Determines how much a TK item increases alertedness, based on minMaxEnemyInFOVByRangePerSec
        public float TKItemMultiplier = 0.8f;
        // Determines how much a TK NPC increases alertedness, based on minMaxEnemyInFOVByRangePerSec
        public float TKRagdollMultiplier = 1.5f;
        // The maximum value for alertedness
        public float maxAlertednessValue = 15f;
        public float ignoreTouchByRagdollChance = 0.25f;
        public float handTouchOnHolsterModifier = 3.2f;
        public float ignoreTouchByItemChance = 0.1f;
        public Vector2 minMaxTouchReactDelay = new Vector2(0f, 0.1f);
        public float pickpocketDetectionChance = 0.75f;
        public Vector2 minMaxPickpocketDetectionDelay = new Vector2(1f, 3f);

        [Header("Attacks")]
        public float defenseColliderRadius = 0.5f;
        public float defenseColliderHeightMultiplier = 1f;

        public enum Sight
        {
            Nothing,
            Creature,
            Target,
        }

    }
}