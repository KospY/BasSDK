using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
namespace ThunderRoad
{
    public class CombinationImbuedMechanism : ThunderBehaviour
    {
        [System.Serializable]
        public struct ImbueCombination
        {
            public ColliderGroup colliderGroup;
            public string imbueId;
        }

        public ImbueCombination[] imbueCombination;
        public bool isOrderedConbination;
        public bool autoResetOnFailure;

        public UnityEvent<Transform> onSuccess;
        public UnityEvent<Transform> onFailure;

        public void OnImbue(ColliderGroup colliderGroup)
        {
        }

        [Button]
        public void ResetMechanism()
        {
        }
    }
}