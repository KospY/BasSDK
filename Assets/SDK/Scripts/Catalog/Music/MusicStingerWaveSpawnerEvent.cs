namespace ThunderRoad
{
    using System.Collections.Generic;

    public class MusicStingerWaveSpawnerEvent : MusicDynamicStingerModule
    {
        public enum WaveEndType
        {
            WaveBegin = 0,
            WaveAnyEndEvent = 1,
            WaveWin = 2,
            WaveLost = 3,
            WaveCancel = 4,
            WaveLoop = 5
        }

        public WaveEndType _waveEventType;

    }
}