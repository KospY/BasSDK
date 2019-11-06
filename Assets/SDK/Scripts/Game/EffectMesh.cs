using UnityEngine;

namespace BS
{
    public class EffectMesh : Effect
    {
        [Header("Mesh Display")]
        public bool meshDisplay;

        [Header("Intensity to mesh size")]
        public bool meshSize;
        public AnimationCurve curveMeshSize;


        public override void Play()
        {
            if (meshDisplay)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        public override void Stop()
        {
            if (meshDisplay)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }


        public override void SetIntensity(float value)
        {
            if (meshSize)
            {
                float meshSizeValue = curveMeshSize.Evaluate(value);
                transform.localScale = new Vector3(meshSizeValue, meshSizeValue, meshSizeValue);
            }
        }
    }
}
