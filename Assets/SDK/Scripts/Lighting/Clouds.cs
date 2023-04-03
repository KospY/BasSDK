using UnityEngine;

namespace ThunderRoad
{
	public class Clouds : MonoBehaviour
    {
        public static Clouds instance
        {
            get
            {
                if (!_instance) _instance = GameObject.FindObjectOfType<Clouds>();
                return _instance;
            }
        }
        protected static Clouds _instance;

        public MeshRenderer meshRenderer
        {
            get
            {
                if (!_meshRenderer) _meshRenderer = this.GetComponent<MeshRenderer>();
                return _meshRenderer;
            }
        }
        protected MeshRenderer _meshRenderer;
    }
}