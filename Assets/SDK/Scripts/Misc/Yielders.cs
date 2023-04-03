using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace ThunderRoad
{
	public static class Yielders
    {
        static Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(100, new FloatComparer());
        static Dictionary<float, WaitForSecondsRealtime> _realTimeInterval = new Dictionary<float, WaitForSecondsRealtime>(100, new FloatComparer());

        static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();

        public static WaitForEndOfFrame EndOfFrame
        {
            get { return _endOfFrame; }
        }

        static WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();

        public static WaitForFixedUpdate FixedUpdate
        {
            get { return _fixedUpdate; }
        }

        public static WaitForSeconds ForSeconds(float seconds)
        {
            WaitForSeconds wfs;
            if (!_timeInterval.TryGetValue(seconds, out wfs))
            {
                wfs = new WaitForSeconds(seconds);
                _timeInterval.Add(seconds, wfs);
            }
            return wfs;
        }

        public static WaitForSecondsRealtime ForRealSeconds(float seconds)
        {
            return new WaitForSecondsRealtime(seconds);
            // Leaving this code here for the future; We need to figure out a better way to "hand out" WaitForSecondsRealtime instances without giving out the same instance to multiple coroutines
            /*
            WaitForSecondsRealtime wfsr;
            if (!_realTimeInterval.TryGetValue(seconds, out wfsr))
                _realTimeInterval.Add(seconds, wfsr = new WaitForSecondsRealtime(seconds));
            return wfsr;
            */
        }
    }
}
