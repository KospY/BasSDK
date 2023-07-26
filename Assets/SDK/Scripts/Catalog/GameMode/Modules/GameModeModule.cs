using System;

namespace ThunderRoad.Modules
{
    public abstract class GameModeModule : Module
    {
        [NonSerialized]
        public GameModeData gameMode;
    }
}
