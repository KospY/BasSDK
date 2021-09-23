using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIWaveSpawner : MonoBehaviour
    {
        public string id;
        public WaveSpawner waveSpawner;

        public int difficulty = 0;
        public float startDelay = 5;
        public GameObject categoryPrefab;
        public GameObject wavePrefab;
        public GameObject wavePageLeft, wavePageRight;
        public GameObject progressPageLeft, progressPageRight;
        public Text titleText;
        public Text descriptionText;
        public Text npcCountText;
        public Text healthModifierText;
        public Button startButton;

        public Button stopButton;
        public Text stateText;

        protected void OnDrawGizmos()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.402f, 0.29f, 0));
        }

    }
}
