using UnityEngine;

namespace Shadowood
{
    [ExecuteInEditMode]
    public class AlphaToCoverageMono : MonoBehaviour
    {
        public void Reset()
        {
            gameObject.name = "AlphaToCoverage";
        }

        [ContextMenu("EnableAlphaToCoverage")]
        public void OnEnable()
        {
            AlphaToCoverage.EnableAlphaToCoverage();
        }

        [ContextMenu("DisableAlphaToCoverage")]
        public void DisableAlphaToCoverage()
        {
            AlphaToCoverage.DisableAlphaToCoverage();
        }
    }
}
