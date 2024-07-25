using System;
using UnityEngine;

namespace ThunderRoad
{
    public enum DeathPenalty
    {
        None,
        LoseGold,
        Permadeath
    }
    public class DeathPenaltyOption : OptionEnum<DeathPenalty>, IGameModeOption
    {
        public DeathPenaltyOption()
        {
            name = "DeathPenalty";
            displayName = "Death Penalty";
            description = "DeathPenaltyDescription";
        }
  // ProjectCore
    }

}
