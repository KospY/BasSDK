using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    [CreateAssetMenu(menuName ="Manikin/Morph Asset")]
    public class ManikinMorphAsset : ScriptableObject
    {
        [System.NonSerialized] public Mesh targetMesh;
        public string morphName;

        public int MorphHash { get { return morphHash; } }
        [SerializeField, HideInInspector]private int morphHash;
        //Let's serialize the target mesh guids and not the reference so it doesn't accidently get included in a build.
        [SerializeField, HideInInspector] private string meshGuid;
        [SerializeField, HideInInspector] private long fileID;

        public Vector3[] deltaVertices;
        public Vector3[] deltaNormals;
        public Vector3[] deltaTangents;

#if UNITY_EDITOR
        private void OnValidate()
        {
            morphHash = Animator.StringToHash(morphName);

            if (!string.IsNullOrEmpty(meshGuid))
            {
                targetMesh = null;
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(meshGuid);
                Object[] objects = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);
                foreach(var obj in objects)
                {
                    if(UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out string guid, out long localid))
                    {
                        if(guid == meshGuid && fileID == localid)
                        {
                            targetMesh = obj as Mesh;
                            break;
                        }
                    }
                }
            }
        }

        public static string[] BlendShapeNamesFromMesh(Mesh mesh)
        {
            if (mesh == null)
                return new string[0];

            int count = mesh.blendShapeCount;
            string[] names = new string[count];
            for(int i = 0; i < count; i++)
            {
                names[i] = mesh.GetBlendShapeName(i);
            }

            return names;
        }

        public bool ExtractBlendShape(string name, Mesh mesh)
        {
            if (mesh == null)
                return false;

            int shapeIndex = mesh.GetBlendShapeIndex(name);
            if (shapeIndex == -1)
                return false;

            //Support only one frame for now.
            deltaVertices = new Vector3[targetMesh.vertexCount];
            deltaNormals = new Vector3[targetMesh.vertexCount];
            deltaTangents = new Vector3[targetMesh.vertexCount];
            mesh.GetBlendShapeFrameVertices(shapeIndex, 0, deltaVertices, deltaNormals, deltaTangents);
            return true;
        }
#endif
    }
}
