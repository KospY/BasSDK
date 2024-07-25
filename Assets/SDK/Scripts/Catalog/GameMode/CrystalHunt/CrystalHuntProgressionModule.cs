using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace ThunderRoad.Modules
{
    public class CrystalHuntProgressionModule : GameModeModule
    {
        public enum EndGameState
        {
            Locked,                 // Progression level < 6
            LockedAndRaidDone,		// Progression level = 6
            DalgarianDoorOpened,    // Progression level >= 7
            KhemenetEnding,         // Progression level >= 7 + After Khemenet Ending
            AnnihilationEnding,     // Progression level >= 7 + After Annihilation Ending + 1st visit to the home
            PostAnnihilationEnding  // Progression level >= 7 + After Annihilation Ending + After visiting home for the 1st time
        }
 //ProjectCore
    }
}
