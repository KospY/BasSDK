using UnityEngine;
#if ProjectCore
using Sirenix.OdinInspector;
using PaintIn3D;
#endif

namespace BS
{
    public class Paintable : MonoBehaviour
    {
        public string shaderProperty = "_BaseMap";
        public float radius = 0.1f;
        public float depth = 1;
        public float opacity = 1;
        public float hardness = 1;
        public float normalFront = 0.2f;
        public float normalBack;
        public float normalFade = 0.01f;

#if ProjectCore
        protected P3dPaintableTexture paintableTexture;
        protected P3dMaterialCloner materialCloner;

        void Awake()
        {
            materialCloner = this.gameObject.AddComponent<P3dMaterialCloner>();
            materialCloner.Activate();
            paintableTexture = this.gameObject.AddComponent<P3dPaintableTexture>();
            paintableTexture.Slot = new P3dSlot(0, shaderProperty);
            paintableTexture.Activate();
        }

        [Button]
        public void Clear()
        {
            paintableTexture.Clear(paintableTexture.Texture, Color.black);
        }

        public void Paint(P3dPaintDecal.Command command, Vector3 position, Quaternion rotation, Texture texture, Color color, MaterialData.PaintBlendMode blendMode)
        {
            float finalOpacity = opacity + (1.0f - opacity);
            float angle = Random.Range(-180.0f, 180.0f);
            command.SetLocation(position, rotation, Vector2.one, radius, texture, depth);
            command.SetMaterial((P3dBlendMode)(int)blendMode, texture, hardness, normalBack, normalFront, normalFade, color, finalOpacity, null);
            P3dPaintableManager.SubmitAll(command, false, -1, -1, null, paintableTexture, null, null);
        }
#endif
    }
}