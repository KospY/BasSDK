using UnityEngine;

#if DUNGEN
using DunGen;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class RoomSunOrientation : MonoBehaviour
    {
        public Quaternion directionalLightRotation;
        protected Light directionalLight;

#if PrivateSDK
        protected Tile tile;
#endif

        private void OnValidate()
        {
            if (!this.gameObject.activeInHierarchy) return;
            if (!Application.isPlaying)
            {
                directionalLight = GetDirectionalLight();
                if (directionalLight)
                {
                    SetDirectionalLightRotation();
                }
            }
        }

        private void Awake()
        {
            directionalLight = GetDirectionalLight();
#if PrivateSDK
            tile = this.GetComponent<Tile>();
            if (tile)
            {
                AdjacentRoomCulling adjacentRoomCulling = GameObject.FindObjectOfType<AdjacentRoomCulling>();
                if (adjacentRoomCulling)
                {
                    adjacentRoomCulling.onTileVisibilityChanged += OnTileVisibilityChanged;
                }
            }
            else
            {
                Debug.LogError("Tile not found in the gameobject!");
            }
#endif
        }

#if PrivateSDK
        protected void OnTileVisibilityChanged(Tile tile)
        {
            if (tile && this.tile == tile)
            {
                // maybe not usefull to change direction light rotation at runtime? Does it change something on reflection?
                SetDirectionalLightRotation();
            }
        }
#endif

        public Light GetDirectionalLight()
        {
            foreach (Light light in GameObject.FindObjectsOfType<Light>())
            {
                if (light.type == LightType.Directional && light.gameObject.scene == this.gameObject.scene)
                {
                    return light;
                }
            }
            Debug.LogError("No directional light found in the scene!");
            return null;
        }

        [Button]
        public void SaveDirectionalLightRotation()
        {
            directionalLight = GetDirectionalLight();
            if (directionalLight)
            {
                directionalLightRotation = directionalLight.transform.rotation;
                Debug.Log("Directional light rotation saved!");
            }
        }

        [Button]
        public void SetDirectionalLightRotation()
        {
            if (directionalLight && directionalLightRotation != Quaternion.identity)
            {
                Debug.Log("Directional light rotation set!");
                directionalLight.transform.rotation = directionalLightRotation;
            }
        }
    }
}