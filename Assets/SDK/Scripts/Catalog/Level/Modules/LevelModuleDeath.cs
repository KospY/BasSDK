using UnityEngine;
using System.Collections;

namespace ThunderRoad
{
    public class LevelModuleDeath : LevelModule
    {
        public Behaviour behaviour = Behaviour.ReloadLevel;
        public float delayBeforeLoad = 10;
        public bool disablePlayerLocomotion = true;

        public enum Behaviour
        {
            LoadHome,
            ReloadLevel,
            ShowDeathMenu,
            Respawn
        }

    }
}
