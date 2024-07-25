using TMPro;
using UnityEngine;

namespace ThunderRoad
{
    [ExecuteInEditMode]
    public class TMPWarper : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private UIText _optionalTextHandler;
        [SerializeField] private AnimationCurve _warpCurve = new();
        [SerializeField] private float _curveScaling = 55.05f;

        private void Awake()
        {
            if (_optionalTextHandler != null)
            { _optionalTextHandler.TextChanged += Refresh; }

            _text.ForceMeshUpdate();
            Refresh();
        }

        private void OnEnable()
        { Refresh(); }

        private void OnValidate()
        { Refresh(); }

#if UNITY_EDITOR
        // Used for previews in editor, will not run in play mode.
        private void Update()
        {
            if (_text == null ||
                Application.isPlaying)
            { return; }

            Refresh();
        }
#endif

        /// <summary>
        /// Refresh the curved text.
        /// </summary>
        public void Refresh()
        { Warp(_curveScaling); }

        /// <summary>
        /// Curve the target text.
        /// </summary>
        private void Warp(float curveScale = 25.0f)
        {
            TMP_TextInfo textInfo = _text.textInfo;

            if (textInfo == null ||
                textInfo.characterCount == 0)
            { return; }

            // Clamp curve values.
            _warpCurve.preWrapMode = WrapMode.Clamp;
            _warpCurve.postWrapMode = WrapMode.Clamp;

            // Mark text dirty.
            _text.havePropertiesChanged = true;
            _text.ForceMeshUpdate();

            float boundsMinX = _text.bounds.min.x;
            float boundsMaxX = _text.bounds.max.x;

            Vector3[] vertices;
            Matrix4x4 matrix;
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                // Ignore invisible characters.
                if (!textInfo.characterInfo[i].isVisible)
                { continue; }

                int vIndex = textInfo.characterInfo[i].vertexIndex;

                // Get the index of the mesh used by this character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                vertices = textInfo.meshInfo[materialIndex].vertices;

                // Calculate the offset to center.
                Vector3 offsetToMidBaseline = new Vector2((vertices[vIndex + 0].x + vertices[vIndex + 2].x) / 2,
                                                       textInfo.characterInfo[i].baseLine);

                // Apply offset to adjust pivot point.
                vertices[vIndex + 0] += -offsetToMidBaseline;
                vertices[vIndex + 1] += -offsetToMidBaseline;
                vertices[vIndex + 2] += -offsetToMidBaseline;
                vertices[vIndex + 3] += -offsetToMidBaseline;

                // Compute the angle of rotation for each character based on the animation curve
                // Character's position relative to the bounds of the mesh.
                float x0 = (offsetToMidBaseline.x - boundsMinX) / (boundsMaxX - boundsMinX);
                float x1 = x0 + 0.0001f;
                float y0 = _warpCurve.Evaluate(x0) * curveScale;
                float y1 = _warpCurve.Evaluate(x1) * curveScale;

                Vector3 horizontal = new(1, 0, 0);
                Vector3 tangent = new Vector3(x1 * (boundsMaxX - boundsMinX) + boundsMinX, y1) - new Vector3(offsetToMidBaseline.x, y0);

                float dot = Mathf.Acos(Vector3.Dot(horizontal, tangent.normalized)) * Mathf.Rad2Deg;
                Vector3 cross = Vector3.Cross(horizontal, tangent);
                float angle = cross.z > 0 ? dot : 360 - dot;

                matrix = Matrix4x4.TRS(new(0, y0, 0), Quaternion.Euler(0, 0, angle), Vector3.one);

                vertices[vIndex + 0] = matrix.MultiplyPoint3x4(vertices[vIndex + 0]);
                vertices[vIndex + 1] = matrix.MultiplyPoint3x4(vertices[vIndex + 1]);
                vertices[vIndex + 2] = matrix.MultiplyPoint3x4(vertices[vIndex + 2]);
                vertices[vIndex + 3] = matrix.MultiplyPoint3x4(vertices[vIndex + 3]);

                vertices[vIndex + 0] += offsetToMidBaseline;
                vertices[vIndex + 1] += offsetToMidBaseline;
                vertices[vIndex + 2] += offsetToMidBaseline;
                vertices[vIndex + 3] += offsetToMidBaseline;

                // Upload the mesh with the revised information
                _text.UpdateVertexData();
            }
        }
    }
}