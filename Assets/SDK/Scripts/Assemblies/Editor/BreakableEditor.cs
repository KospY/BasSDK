using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace ThunderRoad
{
    [CustomEditor(typeof(Breakable))]
    public class BreakableEditor : Editor
    {
        private Breakable breakable;
        private static bool a = true;
        private static Material mat;

        public override void OnInspectorGUI()
        {
            breakable = target as Breakable;
            if (!breakable) return;

            if (GUILayout.Button("Set up handles"))
                breakable.AutoMatchHandles();
            
            if (GUILayout.Button("Check for collider intersection (using 10cm threshold)"))
                CheckForIntersection(0.1f);
            
            if (GUILayout.Button("Check for collider intersection (using 5cm threshold)"))
                CheckForIntersection(0.05f);

            if (GUILayout.Button("Check for collider intersection (using 1cm threshold)"))
                CheckForIntersection(0.01f);
            
            if (GUILayout.Button("Check for collider intersection (no threshold)"))
                CheckForIntersection();

            breakable.editingBreakpointsThroughEditor =
                EditorGUILayout.Toggle("Edit Breakpoints", breakable.editingBreakpointsThroughEditor);

            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }

        private void CheckForIntersection(float threshold = 0f)
        {
            if (!breakable) return;
            breakable.RetrieveSubItems();

            Debug.Log($"Checking for intersection on {breakable.name}!", breakable);
            Debug.Log("------------------");

            breakable.brokenObjectsHolder.SetActive(true);

            var colliders = new List<(Item item, Collider collider)>();
            var alreadyChecked = new Dictionary<Collider, List<Collider>>();
            foreach (var item in breakable.subBrokenItems)
            {
                foreach (var c in item.GetComponentsInChildren<Collider>(true))
                {
                    colliders.Add((item, c));
                }
            }

            Vector3 direction;
            float distance;
            bool hasIntersected = false;
            int intersectionAmount = 0;
            foreach (var c1 in colliders)
            {
                foreach (var c2 in colliders)
                {
                    if (c1 == c2) continue;
                    if (c1.item == c2.item) continue;
                    
                    var i1 = breakable.subBrokenItems.IndexOf(c1.item);
                    var i2 = breakable.subBrokenItems.IndexOf(c2.item);

                    var color1 = "#" +
                                 ColorUtility.ToHtmlStringRGB(
                                     Color.HSVToRGB(i1 / (float) breakable.subBrokenItems.Count, .35f, 1f));
                    var color2 = "#" +
                                 ColorUtility.ToHtmlStringRGB(
                                     Color.HSVToRGB(i2 / (float) breakable.subBrokenItems.Count, .35f, 1f));

                    if(!alreadyChecked.ContainsKey(c1.collider))
                        alreadyChecked.Add(c1.collider, new List<Collider>());
                    
                    if(!alreadyChecked.ContainsKey(c2.collider))
                        alreadyChecked.Add(c2.collider, new List<Collider>());

                    if (alreadyChecked[c1.collider].Contains(c2.collider)) continue;
                    
                    alreadyChecked[c1.collider].Add(c2.collider);
                    alreadyChecked[c2.collider].Add(c1.collider);

                    var isIntersecting = Physics.ComputePenetration(c1.collider, c1.collider.transform.position,
                        c1.collider.transform.rotation,
                        c2.collider, c2.collider.transform.position, c2.collider.transform.rotation, out direction,
                        out distance);
                    
                    if (isIntersecting && distance >= threshold)
                    {
                        Debug.Log(
                            $"<color={color1}>{c1.collider.name}</color> is intersecting with <color={color2}>{c2.collider.name}!</color>" +
                            $" | <color={color1}>{c1.item.name}</color> ↔ <color={color2}>{c2.item.name}</color>",
                            c1.collider);
                        Debug.Log(
                            $"<color={color2}>{c2.collider.name}</color> is intersecting with <color={color1}>{c1.collider.name}!</color>" +
                            $" | <color={color1}>{c1.item.name}</color> ↔ <color={color2}>{c2.item.name}</color>",
                            c2.collider);
                        Debug.Log(" ");

                        hasIntersected = true;
                        intersectionAmount++;
                    }
                }
            }
            
            breakable.brokenObjectsHolder.SetActive(false);

            Debug.Log(!hasIntersected ? "No intersection!" : $"Found {intersectionAmount} intersections");
        }

        private void OnSceneGUI()
        {
            breakable = target as Breakable;
            if (!breakable) return;
            if (!breakable.brokenObjectsHolder) return;

            CheckBreakPoints();

            if (breakable.editingBreakpointsThroughEditor) return;

            var rbs = breakable.brokenObjectsHolder.GetComponentsInChildren<Rigidbody>(true);
            var center = breakable.brokenObjectsHolder.transform.position;
            var orderedRbs = rbs.OrderBy(rb => Vector3.SignedAngle(rb.transform.position, center, Vector3.forward))
                .ToArray();

            var globalBounds = new Bounds();
            bool init = false;
            foreach (var rb in orderedRbs)
            {
                var renderers = rb.GetComponentsInChildren<Renderer>(true);
                foreach (var r in renderers)
                {
                    if (!init)
                    {
                        globalBounds = r.bounds;
                        init = true;
                    }

                    globalBounds.Encapsulate(r.bounds);
                }
            }

            if (!mat)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things. In this case, we just want to use
                // a blend mode that inverts destination colors.
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                mat = new Material(shader) {hideFlags = HideFlags.HideAndDontSave};

                // Turn off backface culling, depth writes, depth test.
                mat.SetInt("_ZWrite", 0);
                mat.SetInt("_ZTest", (int) CompareFunction.Always);
            }

            for (var i = 0; i < orderedRbs.Length; i++)
            {
                var rb = orderedRbs[i];
                var hasHandle = rb.GetComponentsInChildren<Handle>();
                var color = new Color(.7f, .7f, .7f, .5f);
                var handleLink = breakable.handleLinks.FirstOrDefault(hl =>
                    hasHandle.Contains(hl.handleMain) || hasHandle.Contains(hl.handleSecondary));

                if (handleLink != null)
                {
                    for (int j = 0; j < breakable.handleLinks.Length; j++)
                    {
                        if (breakable.handleLinks[j] == handleLink)
                        {
                            color = Color.HSVToRGB(j / (float) breakable.handleLinks.Length, 1f, 1f);
                            break;
                        }
                    }
                }

                Gizmos.color = color;
                mat.SetColor("_Color", color);

                var angle = (i + 1) / ((float) orderedRbs.Length + 1) * (Mathf.PI);

                var meshes = rb.GetComponentsInChildren<MeshFilter>(true);

                for (var j = 0; j < meshes.Length; j++)
                {
                    var filter = meshes[j];
                    if(!filter) continue;
                    if(!filter.sharedMesh) continue;
                    
                    var start = filter.transform.TransformPoint(filter.sharedMesh.bounds.center);

                    var p = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) *
                        (globalBounds.extents.magnitude * 3f);

                    Handles.DrawBezier(start, p,
                        start + Vector3.up * globalBounds.extents.magnitude,
                        p - Vector3.up * globalBounds.extents.magnitude,
                        color, EditorGUIUtility.whiteTexture, 1f);

                    var m = Matrix4x4.TRS(p - filter.sharedMesh.bounds.center, filter.transform.rotation,
                        filter.transform.lossyScale);

                    mat.SetPass(0);
                    Graphics.DrawMeshNow(filter.sharedMesh, m);
                }
            }

            if (a)
            {
                EditorUtility.SetDirty(target);
                a = false;
            }
        }

        private void CheckBreakPoints()
        {
            if (!breakable.editingBreakpointsThroughEditor) return;

            for (int i = 0; i < breakable.breakPoints.Count; i++)
            {
                var bp = breakable.breakPoints[i];

                EditorGUI.BeginChangeCheck();
                Vector3 newTargetPosition =
                    Handles.PositionHandle(breakable.transform.TransformPoint(bp.center), Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(breakable, $"Change break {i} center position");
                    bp.center = breakable.transform.InverseTransformPoint(newTargetPosition);
                }
            }
        }
    }
}