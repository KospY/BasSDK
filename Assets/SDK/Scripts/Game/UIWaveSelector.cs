using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIWaveSelector : MonoBehaviour
    {
        public string id;
        public SpawnLocation spawnLocation;

        void OnValidate()
        {
            if (id == null || id == "") id = this.gameObject.scene.name;
        }

#if ProjectCore

        UIPageWaves uiPageWaves;

        protected void Awake()
        {
            uiPageWaves = Instantiate(Resources.Load("UI/WaveSelector", typeof(UIPageWaves)), this.transform.position, this.transform.rotation, this.transform) as UIPageWaves;
            uiPageWaves.name = "WaveSelector";
            uiPageWaves.id = id;
            uiPageWaves.spawnLocation = spawnLocation;
            uiPageWaves.Init();
        }

        protected void Start()
        {
            uiPageWaves.RegisterModuleWave();
            this.gameObject.SetActive(false);
        }

#endif

        protected void OnDrawGizmos()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.402f, 0.29f, 0));
        }
    }
}
