using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class BowString : MonoBehaviour
    {
        public float maxDrawDistance = 0.5f;
        public new Animation animation;
        public float minPull = 0.01f;
        public float pullMultiplier = 2.5f;
        public float pullOffset = 0;

        public float unnockVelocity = 4;
        public float ungrabExceedDistance = 0.1f;
        public float unnockOffset = 0.15f;
        public float nockUnockDelay = 0.5f;

        public AudioContainer audioContainerShoot;
        public AudioContainer audioContainerDraw;
        public AudioClip audioClipString;

    }
}