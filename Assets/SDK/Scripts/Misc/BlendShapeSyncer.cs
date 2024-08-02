using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class BlendShapeSyncer : ThunderBehaviour
    {
        [Serializable]
        public class ShapeNameCleaner
        {
            public enum StringMethod
            {
                ToLower,
                ToUpper,
                Split,
                Replace,
            }

            public StringMethod method;
#if ODIN_INSPECTOR
            [ShowIf("method", StringMethod.Split)]
#endif
            public char splitChar;
#if ODIN_INSPECTOR
            [ShowIf("method", StringMethod.Split)]
#endif
            public Side splitSide;
#if ODIN_INSPECTOR
            [ShowIf("method", StringMethod.Replace)]
#endif
            public string stringToReplace;
#if ODIN_INSPECTOR
            [ShowIf("method", StringMethod.Replace)]
#endif
            public string replacement;

            public string Clean(string input)
            {
                switch (method)
                {
                    case StringMethod.ToLower: return input.ToLower();
                    case StringMethod.ToUpper: return input.ToUpper();
                    case StringMethod.Split: return input.Split(new char[] { splitChar }, 2)[splitSide == Side.Left ? 0 : 1];
                    case StringMethod.Replace: return input.Replace(stringToReplace, replacement);
                }
                return input;
            }
        }

#if ODIN_INSPECTOR
        [TableList()]
#endif
        public List<ShapeNameCleaner> selfNameCleaning = new List<ShapeNameCleaner>();
        public SkinnedMeshRenderer driverSMR;
#if ODIN_INSPECTOR
        [TableList()]
#endif
        public List<ShapeNameCleaner> driverNameCleaning = new List<ShapeNameCleaner>();

        public override ManagedLoops EnabledManagedLoops => ManagedLoops.FixedUpdate;

        private SkinnedMeshRenderer skinnedMeshRenderer;


#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        private List<string> selfCleanedNames;
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        private List<string> driverCleanedNames;
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        private Dictionary<int, int> blendShapeDictionary;

        private void Start()
        {
            SetBlendShapeDictionary();
        }

        [Button]
        private void SetBlendShapeDictionary()
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            blendShapeDictionary = new Dictionary<int, int>();
            selfCleanedNames = new List<string>();
            driverCleanedNames = new List<string>();
            List<int> matchedIndices = new List<int>();
            for (int i = 0; i < skinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
            {
                string shapeName = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i);
                foreach (ShapeNameCleaner cleaner in selfNameCleaning) shapeName = cleaner.Clean(shapeName);
                selfCleanedNames.Add(shapeName);
            }
            for (int i = 0; i < driverSMR.sharedMesh.blendShapeCount; i++)
            {
                string cleanedName = driverSMR.sharedMesh.GetBlendShapeName(i);
                foreach (ShapeNameCleaner cleaner in driverNameCleaning) cleanedName = cleaner.Clean(cleanedName);
                driverCleanedNames.Add(cleanedName);
                if (!selfCleanedNames.Contains(cleanedName) || blendShapeDictionary.ContainsKey(i)) continue;
                int matchedIndex = selfCleanedNames.IndexOf(cleanedName);
                blendShapeDictionary.Add(i, matchedIndex);
                matchedIndices.Add(matchedIndex);
            }
            for (int i = 0; i < selfCleanedNames.Count; i++)
            {
                if (matchedIndices.Contains(i)) continue;
                Debug.LogWarning($"Missed blendshape {skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i)} ({selfCleanedNames[i]}) in the matching process: You may need to adjust your name cleaning process if this blendshape is important!");
            }
        }

        protected internal override void ManagedFixedUpdate()
        {
            foreach (KeyValuePair<int, int> blendShapePairing in blendShapeDictionary)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(blendShapePairing.Value, driverSMR.GetBlendShapeWeight(blendShapePairing.Key));
            }
        }
    }
}
