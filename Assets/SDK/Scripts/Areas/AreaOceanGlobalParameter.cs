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
            if (connectedArea == null)
            {
                Apply();
                return;
            }

            if (visibleFromGate)
            {
                Apply();
                return;
            }

            if (isInArea && !inGateWay)
            {
                Apply();
                return;
            }

            AreaOceanGlobalParameter connectedOcean;
            if (!connectedArea.TryGetGlobalParameter<AreaOceanGlobalParameter>(out connectedOcean))
            {
                Apply();
                return;
            }

            if (isInArea && inGateWay && !connectedOcean.visibleFromGate)
            {
                Apply();
                return;
            }
        }

        public void Apply()
        {
            // Todo change the main ocean
        }

        public override bool IsCompatible(AreaGlobalParameter other)
        {
            if(other is AreaOceanGlobalParameter otherOceanGlobalParameter)
            {
                /*if (base.IsCompatible(other))
                {
                    return true;
                }*/

                // todo check if both ocean are the same
                return false;
            }

            return true;
        }
        public override AreaGlobalParameter GetParameterFor(SpawnableArea spawnableArea)
        {
            return new AreaOceanGlobalParameter(visibleFromGate);
        }

    }
}
