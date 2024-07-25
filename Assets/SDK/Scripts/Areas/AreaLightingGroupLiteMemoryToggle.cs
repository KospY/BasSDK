using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Areas/AreaLightingGroupLiteMemoryToggle.html")]
    public class AreaLightingGroupLiteMemoryToggle : MonoBehaviour, ILiteMemoryToggle
    {
        private LightingGroup _lightingGroup;
        private SpawnableArea _spawnableArea;
        private bool _useToggle = true;

        public void SetLiteMemory(bool isInLiteMemory)
        {
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

                if (_spawnableArea.LightingPresetAddressLocation == null)
                {
                    _useToggle = false;
                    return;
                }
            }

        }

        private void OnPresetLoaded(LightingPreset preset)
        {
            _lightingGroup.ApplyPresetWithoutSceneSettings(preset);
            if (_spawnableArea != null && _spawnableArea == AreaManager.Instance.CurrentArea)
            {
                // current area apply scene setting
                _lightingGroup.ApplySceneSettings();
            }
        }

        public MonoBehaviour GetMonoBehaviourReference()
        {
            return this;
        }

    }
}