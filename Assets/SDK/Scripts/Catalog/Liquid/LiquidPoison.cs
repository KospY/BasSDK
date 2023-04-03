using System;

#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class LiquidPoison : LiquidData
    {
        public float healthLoss = 5;

    }
}