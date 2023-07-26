using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public class AreaLightingGroupLiteMemoryToggle : MonoBehaviour, ILiteMemoryToggle
    {
        private LightingGroup _lightingGroup;
        private SpawnableArea _spawnableArea;
        private bool _useToggle = true;

        public void SetLiteMemory(bool isInLiteMemory)
        {
            if (!_useToggle) return;

            InitLighting();
            InitSpawnableArea();

            if (!_useToggle) return;

            if (isInLiteMemory)
            {
                if (_lightingGroup.lightingPreset != null)
                {
                    _lightingGroup.ClearLightingPreset();
                    _spawnableArea.AreaData.ReleaseLightingPreset();
                }
            }
            else
            {
                if (_lightingGroup.lightingPreset == null)
                {
                    _spawnableArea.AreaData.GetLightingPreset(OnPresetLoaded);
                }
            }
        }

        private void InitLighting()
        {
            if (_lightingGroup == null)
            {
                _lightingGroup = GetComponent<LightingGroup>();
                if (_lightingGroup == null)
                {
                    _useToggle = false;
                }
            }
        }

        private void InitSpawnableArea()
        {
            if (_spawnableArea == null)
            {
                Area area = GetComponentInParent<Area>();
                if (area == null)
                {
                    _useToggle = false;
                    return;
                }

                _spawnableArea = area.spawnableArea;
                if (_spawnableArea == null)
                {
                    _useToggle = false;
                    return;
                }

                if(_spawnableArea.LightingPresetAddressLocation == null)
                {
                    _useToggle = false;
                    return;
                }
            }

        }

        private void OnPresetLoaded(LightingPreset preset)
        {
            _lightingGroup.ApplyPresetWithoutSceneSettings(preset);
        }

        public MonoBehaviour GetMonoBehaviourReference()
        {
            return this;
        }

    }
}