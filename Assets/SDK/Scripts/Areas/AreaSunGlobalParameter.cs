using System;

namespace ThunderRoad
{
    [Serializable]
    public class AreaSunGlobalParameter : AreaGlobalParameter
    {
        public bool isZeroIntensity;
        public AreaSunGlobalParameter() : base() { }
        public AreaSunGlobalParameter(bool visibleFromGate) : base(visibleFromGate) 
        {
        }
        
        public override void Apply(bool isInArea, bool inGateWay, SpawnableArea connectedArea)
        {
        }
        public void Apply()
        {
        }

        public override bool IsCompatible(AreaGlobalParameter other)
        {

            return true;
        }

        public override AreaGlobalParameter GetParameterFor(SpawnableArea spawnableArea)
        {
return default;
        }
    }
}