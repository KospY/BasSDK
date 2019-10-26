using UnityEngine;

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
    }
}