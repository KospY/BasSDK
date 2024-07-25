using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public class AnimationFxPlayer : MonoBehaviour
    {
        public FxController[] controllers;

        public delegate void PlayEvent(AnimationFxPlayer player, int index, FxController controller);

        public event PlayEvent OnPlayEvent;

    }
}
