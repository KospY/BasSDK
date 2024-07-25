using System;
namespace ThunderRoad
{
    [Serializable]
    public class AreaOceanGlobalParameter : AreaGlobalParameter
    {
        public AreaOceanGlobalParameter() : base() { }
        public AreaOceanGlobalParameter(bool visibleFromGate) : base(visibleFromGate) { }
        public override void Apply(bool isInArea, bool inGateWay, SpawnableArea connectedArea)
        {
        }

        public void Apply()
        {
            // Todo change the main ocean
        }

        public override bool IsCompatible(AreaGlobalParameter other)
        {

            return true;
        }
        public override AreaGlobalParameter GetParameterFor(SpawnableArea spawnableArea)
        {
            return new AreaOceanGlobalParameter(visibleFromGate);
        }

    }
}
