using UnityEngine;

namespace ThunderRoad
{
	public class Clouds : MonoBehaviour
    {
        public static Clouds instance
        {
            get
            {
                return _instance;
            }
        }
        protected static Clouds _instance;

        void Awake()
        {
            if(!_instance) _instance = this;
            else if(_instance != this) Destroy(this.gameObject);
        }

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