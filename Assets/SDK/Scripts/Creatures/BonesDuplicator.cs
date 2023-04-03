using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

public class BonesDuplicator : MonoBehaviour
{

    void Start()
    {
        CopyBones();
    }

    // Your character's Shape (containing Skinned Mesh Renderer)
    [SerializeField] private GameObject _sourceHumanBody;

    [Button]
    private void CopyBones()
    {
        var sourceRenderer = _sourceHumanBody.GetComponent<SkinnedMeshRenderer>();
        var targerRenderer = GetComponent<SkinnedMeshRenderer>();

        targerRenderer.bones = sourceRenderer.bones.Where(b => targerRenderer.bones.Any(t => t.name.Equals(b.name)))
            .ToArray();
    }

    [Button]
    private void CopyBonesWithDictionary()
    {
        SkinnedMeshRenderer targetRenderer = _sourceHumanBody.GetComponent<SkinnedMeshRenderer>();

        Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
        foreach (Transform bone in targetRenderer.bones)
        {
            boneMap[bone.name] = bone;
        }

        SkinnedMeshRenderer thisRenderer = GetComponent<SkinnedMeshRenderer>();
        Transform[] boneArray = thisRenderer.bones;
        for (int idx = 0; idx < boneArray.Length; ++idx)
        {
            string boneName = boneArray[idx].name;
            if (false == boneMap.TryGetValue(boneName, out boneArray[idx]))
            {
                Debug.LogError("failed to get bone: " + boneName);
                Debug.Break();
            }
        }
        thisRenderer.bones = boneArray;
    }
}