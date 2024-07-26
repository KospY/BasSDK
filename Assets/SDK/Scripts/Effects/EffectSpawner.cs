using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	public class EffectSpawner : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string effectId;

        public bool autoIntensity;
#if ODIN_INSPECTOR
        [ShowIf("autoIntensity")]
#endif
        public AnimationCurve intensityCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        protected float playTime;

        public bool spawnOnStart = true;
        public bool editorLoad = true;
        
        [Range(0, 1)]
        public float intensity;

        [Range(0, 1)]
        public float speed;

        public bool useMainGradient;
        [GradientUsage(true)]
        public Gradient mainGradient;

        public bool useSecondaryGradient;
        [GradientUsage(true)]
        public Gradient secondaryGradient;

        public Transform source;
        public Transform target;

        public Mesh mesh;
        public Renderer mainRenderer;
        public Renderer secondaryRenderer;
#pragma warning disable 0109 // unity this.collider is obsolete
        public new Collider collider;
#pragma warning restore 0109
        
        
#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Category.Effect);
        }
#endif
        
 // ProjectCore
        
        [Button]
        public void Stop()
        {
            
        }
        
        [Button]
        public void Spawn()
        {
        }
    }
}
