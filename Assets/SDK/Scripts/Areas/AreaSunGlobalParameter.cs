using System;
using UnityEngine;

namespace ThunderRoad
{
    [Serializable]
    public class AreaSunGlobalParameter : AreaGlobalParameter
    {
        public Color dirLightColor = new Color(1, 0.8687521f, 0.7122642f);
        public float dirLightIntensity = 2;
        public float dirLightIndirectMultiplier = 1;
        public Quaternion directionalLightLocalRotation;

        public AreaSunGlobalParameter() : base() { }
        public AreaSunGlobalParameter(bool visibleFromGate, 
                                        Color dirLightColor,
                                        float dirLightIntensity,
                                        float dirLightIndirectMultiplier,
                                        Quaternion directionalLightLocalRotation
                                        ) : base(visibleFromGate) 
        {
            this.dirLightColor = dirLightColor;
            this.dirLightIntensity = dirLightIntensity;
            this.dirLightIndirectMultiplier = dirLightIndirectMultiplier;
            this.directionalLightLocalRotation = directionalLightLocalRotation;
        }
        
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

            AreaSunGlobalParameter connectedSun;
            if(!connectedArea.TryGetGlobalParameter<AreaSunGlobalParameter>(out connectedSun))
            {
                Apply();
                return;
            }

            if(isInArea && inGateWay && !connectedSun.visibleFromGate)
            {
                Apply();
                return;
            }
        }
        public void Apply()
        {
            if (RenderSettings.sun)
            {
                RenderSettings.sun.color = dirLightColor;
                RenderSettings.sun.intensity = dirLightIntensity;
                RenderSettings.sun.bounceIntensity = dirLightIndirectMultiplier;
                RenderSettings.sun.transform.rotation = directionalLightLocalRotation;
            }
        }

        public override bool IsCompatible(AreaGlobalParameter other)
        {
            if (other is AreaSunGlobalParameter sunParameter)
            {
                /*if (base.IsCompatible(other))
                {
                    return true;
                }*/

                return dirLightColor.Equals(sunParameter.dirLightColor)
                    && dirLightIntensity == sunParameter.dirLightIntensity
                    && dirLightIndirectMultiplier == sunParameter.dirLightIndirectMultiplier
                    && directionalLightLocalRotation.Equals(sunParameter.directionalLightLocalRotation);
            }

            return true;
        }

        public override AreaGlobalParameter GetParameterFor(SpawnableArea spawnableArea)
        {
            Quaternion localRotation = AreaRotationHelper.GetRotationQuaternionFromRotation(spawnableArea.Rotation) * directionalLightLocalRotation;
            return new AreaSunGlobalParameter( visibleFromGate, dirLightColor, dirLightIntensity, dirLightIndirectMultiplier, localRotation);
        }
    }
}