using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ThunderRoad
{
    [RequireComponent(typeof(Animator))]
    public class FaceAnimator : ThunderBehaviour
    {
        public enum Expression
        {
            Angry,
            Attack,
            Confusion,
            Death,
            Fear,
            Happy,
            Intrigue,
            Neutral,
            Pain,
            Sad,
            Surprise,
            Tired,
        }

        public class SmoothDampTarget
        {
            public int floatCount { get; protected set; }

            // stored fields
            public float[] target;
            public float[] current;
            public float[] velocity;

            // ephemeral fields
            public float[] change_values;
            public float[] modified_target;
            public float[] temp_values;
            public float[] output_values;
            public float[] orig_minus_current;
            public float[] out_minus_orig;

            public SmoothDampTarget(int count)
            {
                floatCount = count;
                target = new float[count];
                current = new float[count];
                velocity = new float[count];

                change_values = new float[count];
                modified_target = new float[count];
                temp_values = new float[count];
                output_values = new float[count];
                orig_minus_current = new float[count];
                out_minus_orig = new float[count];
            }

            public void ZeroEphemerals()
            {
                for (int i = 0; i < floatCount; i++)
                {
                    change_values[i] = 0f;
                    modified_target[i] = 0f;
                    temp_values[i] = 0f;
                    output_values[i] = 0f;
                    orig_minus_current[i] = 0f;
                    out_minus_orig[i] = 0f;
                }
            }
        }

        public static Expression[] expressionList
        {
            get
            {
                if (_expressionList.IsNullOrEmpty()) _expressionList = (Expression[])Enum.GetValues(typeof(Expression));
                return _expressionList;
            }
        }
        private static Expression[] _expressionList;

        public float expressionChangeSpeedMax = 15f;
        public float varianceChangeSpeedMax = 1f;
        public float smoothTime = 0.15f;
        public bool autoUpdate = false;
        public AnimatorOverrideController animatorOverrideController;
        public AnimationClip overrideA;
        public AnimationClip overrideB;

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public bool animated { get; protected set; }
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public Expression currentPrimaryExpression
        {
            get => _currentPrimaryExpression;
            protected set
            {
                _currentPrimaryExpression = value;
                expressionValues.target = new float[expressionList.Length];
                expressionValues.target[(int)value] = 1f;
            }
        }
        private Expression _currentPrimaryExpression;
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public Expression lastPrimaryExpression { get; protected set; }
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public bool customExpression { get; protected set; }
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public SmoothDampTarget expressionValues { get; protected set; }
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public SmoothDampTarget varianceValues { get; protected set; }
        public Animator animator { get; protected set; }
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public bool brainDriven = false;
        protected AnimatorOverrideController runtimeAnimatorOverrideController;
        protected KeyValuePair<AnimationClip, AnimationClip>[] animationClipOverrides;

        protected static int[] expressionHashes = null;
        protected int hashDynamicExpression, hashLoopDynamic;
        protected float animationEndTime = 0f;
        protected float? neutralExpressionTime = null;

        private void Start()
        {
            animator = GetComponent<Animator>();
            runtimeAnimatorOverrideController = new AnimatorOverrideController(animatorOverrideController.runtimeAnimatorController);
            runtimeAnimatorOverrideController.name = "FacialOverrideAnimator";
            List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            animatorOverrideController.GetOverrides(overrides);
            List<KeyValuePair<AnimationClip, AnimationClip>> newOverrides = new List<KeyValuePair<AnimationClip, AnimationClip>>()
            {

                new KeyValuePair<AnimationClip, AnimationClip> (overrideA, overrideA),
                new KeyValuePair<AnimationClip, AnimationClip> (overrideB, overrideB),
            };
            foreach (KeyValuePair<AnimationClip, AnimationClip> clipPair in overrides)
            {
                if (clipPair.Key == overrideA || clipPair.Key == overrideB) continue;
                newOverrides.Add(clipPair);
            }
            animationClipOverrides = newOverrides.ToArray();
            animator.runtimeAnimatorController = runtimeAnimatorOverrideController;
            runtimeAnimatorOverrideController.ApplyOverrides(overrides);
            expressionValues = new SmoothDampTarget(expressionList.Length);
            varianceValues = new SmoothDampTarget(expressionList.Length);
            if (expressionHashes == null) expressionHashes = new int[expressionList.Length];
            for (int i = 0; i < expressionList.Length; i++)
            {
                Expression expression = expressionList[i];
                expressionHashes[(int)expression] = Animator.StringToHash(expression.ToString());
                expressionValues.target[(int)expression] = animator.GetFloat(expressionHashes[(int)expression]);
                expressionValues.current[(int)expression] = animator.GetFloat(expressionHashes[(int)expression]);
            }
            hashDynamicExpression = Animator.StringToHash("DynamicExpression");
            hashLoopDynamic = Animator.StringToHash("LoopDynamic");
            currentPrimaryExpression = Expression.Neutral;
        }

        public delegate void ExpressionChange(Expression previousExpression, Expression newExpression);
        public event ExpressionChange OnExpressionChanged;

        [Button]
        public void SetExpression(Expression expression) => SetExpression(expression, null);

        public bool SetExpression(Expression expression, float? resetNeutralTime = null)
        {
            if (expression == currentPrimaryExpression) return false;
            lastPrimaryExpression = currentPrimaryExpression;
            currentPrimaryExpression = expression;
            neutralExpressionTime = resetNeutralTime != null ? Time.time + resetNeutralTime : null;
            customExpression = false;
            OnExpressionChanged?.Invoke(lastPrimaryExpression, expression);
            return true;
        }

        public void SetCustomExpressionValues(float[] targets)
        {
            if (targets.IsNullOrEmpty() || targets.Length != expressionValues.target.Length) return;
            expressionValues.target = targets;
            customExpression = true;
        }

        [Button]
        public void PlayAnimation(AnimationClip clip) => PlayAnimation(clip, false);

        public void PlayAnimation(AnimationClip clip, bool loop)
        {
            animator.SetBool(hashLoopDynamic, loop);
            int target = animator.GetInteger(hashDynamicExpression) == 1 ? 2 : 1;
            animationClipOverrides[target - 1] = new KeyValuePair<AnimationClip, AnimationClip>(animationClipOverrides[target - 1].Key, clip);
            runtimeAnimatorOverrideController.ApplyOverrides(animationClipOverrides);
            animator.SetInteger(hashDynamicExpression, target);
            animated = true;
            animationEndTime = Time.time + clip.length;
            StartCoroutine(AnimationEnd(animationEndTime));
        }

        protected IEnumerator AnimationEnd(float endTime)
        {
            yield return Yielders.ForSeconds(endTime - Time.time);
            if (!endTime.IsApproximately(animationEndTime) && endTime < animationEndTime) yield break;
            animated = false;
        }

        [Button]
        public void StopAnimation()
        {
            animator.SetInteger(hashDynamicExpression, 0);
            animator.SetBool(hashLoopDynamic, false);
            animated = false;
        }

        public void SmoothDampChange(SmoothDampTarget values, float maxChangeSpeed)
        {
            // This code is based off the Unity CS reference for Vector3.SmoothDamp, in order to emulate the same motion that method produces, modified to handle what is effectively a Vector12

            // Based on Game Programming Gems 4 Chapter 1.10
            smoothTime = Mathf.Max(0.0001F, smoothTime);
            float omega = 2F / smoothTime;

            float x = omega * Time.deltaTime;
            float exp = 1F / (1F + x + 0.48F * x * x + 0.235F * x * x * x);

            values.ZeroEphemerals();
            for (int i = 0; i < values.floatCount; i++)
            {
                values.modified_target[i] = values.target[i];
                values.change_values[i] = values.current[i] - values.target[i];
            }

            // Clamp maximum speed
            float maxChange = maxChangeSpeed * smoothTime;

            float maxChangeSq = maxChange * maxChange;

            float sqrMag = 0;
            for (int i = 0; i < values.change_values.Length; i++)
            {
                sqrMag += values.change_values[i] * values.change_values[i];
            }
            float orig_sum_value = 0f;
            float mag = (float)Math.Sqrt(sqrMag);
            float deltaTime = Time.deltaTime;
            for (int i = 0; i < values.floatCount; i++)
            {
                if (sqrMag > maxChangeSq)
                {
                    values.change_values[i] = values.change_values[i] / mag * maxChange;
                }
                values.modified_target[i] = values.current[i] - values.change_values[i];
                values.temp_values[i] = (values.velocity[i] + omega * values.change_values[i]) * deltaTime;
                values.velocity[i] = (values.velocity[i] - omega * values.temp_values[i]) * exp;
                values.output_values[i] = values.modified_target[i] + (values.change_values[i] + values.temp_values[i]) * exp;
                values.orig_minus_current[i] = values.target[i] - values.current[i];
                values.out_minus_orig[i] = values.output_values[i] - values.target[i];
                orig_sum_value += values.orig_minus_current[i] * values.out_minus_orig[i];
            }
            for (int i = 0; i < values.floatCount; i++)
            {
                if (orig_sum_value > 0)
                {
                    values.output_values[i] = values.target[i];
                    values.velocity[i] = (values.output_values[i] - values.target[i]) / Time.deltaTime;
                }
                values.current[i] = values.output_values[i];
            }
        }

        public override ManagedLoops EnabledManagedLoops => ManagedLoops.Update;

        protected internal override void ManagedUpdate()
        {
            base.ManagedUpdate();
            AutoUpdates();
        }

        public void AutoUpdates()
        {
            if (!autoUpdate) return;
            if (neutralExpressionTime != null && Time.time >= neutralExpressionTime.Value)
            {
                SetExpression(Expression.Neutral);
            }
            if (brainDriven) return;
            SmoothDampChange(expressionValues, expressionChangeSpeedMax);
            UpdateAnimator();
        }

        public void UpdateAnimator()
        {
            for (int i = 0; i < expressionList.Length; i++)
            {
                Expression expression = expressionList[i];
                float value = expressionValues.current[(int)expression] + varianceValues.current[(int)expression];
                animator.SetFloat(expressionHashes[(int)expression], value);
            }
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (bindings == null) return;
            foreach (SerializeableBinding sb in bindings.allBindables) sb.SetGameObject(gameObject);
#endif
        }

#if UNITY_EDITOR
    [Serializable]
        public class SerializeableBinding
        {
            public string propertyName;
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetTargetProperties))]
#endif
            public string coorespondingProperty;
            private GameObject gameObject;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetTargetProperties()
            {
                List<ValueDropdownItem<string>> options = new List<ValueDropdownItem<string>>();
                if (gameObject == null)
                {
                    options.Add(new ValueDropdownItem<string>("ASSIGN TO FACE ANIMATOR", "ASSIGN TO FACE ANIMATOR"));
                    return options;
                }
                foreach (EditorCurveBinding binding in AnimationUtility.GetAnimatableBindings(gameObject, gameObject))
                {
                    if (!binding.propertyName.ToLower().Contains("blendshape")) continue;
                    if (binding.propertyName.ToLower().Contains("m_blendshapes")) continue;
                    options.Add(new ValueDropdownItem<string>(binding.propertyName, binding.propertyName));
                }
                return options;
            }
#endif

            public void SetGameObject(GameObject g) => gameObject = g;

            public SerializeableBinding(string n, GameObject g)
            {
                propertyName = n;
                gameObject = g;
            }
        }

        [Header("FaceActor clip conversion")]
        public List<AnimationClip> convertClips = new List<AnimationClip>();

        [Button]
        public void GetAllBindables()
        {
            if (bindings == null) return;
            bindings.allBindables = new List<SerializeableBinding>();
            foreach (EditorCurveBinding binding in AnimationUtility.GetAnimatableBindings(gameObject, gameObject))
            {
                if (!binding.propertyName.ToLower().Contains("m_blendshapes")) continue;
                bindings.allBindables.Add(new SerializeableBinding(binding.propertyName, gameObject));
            }
        }

        [Button]
        public void BindableRename(string replace)
        {
            if (bindings == null) return;
            string[] split = replace.Split(':');
            string from = split[0];
            string to = split[1];
            foreach (SerializeableBinding binding in bindings.allBindables)
            {
                binding.coorespondingProperty = binding.coorespondingProperty.Replace(from, to);
            }
        }

        [Button]
        public void FixCurveNames(string replace)
        {
            string[] split = replace.Split(':');
            string from = split[0];
            string to = split[1];
            animator = GetComponent<Animator>();
            for (int c = convertClips.Count - 1; c >= 0; c--)
            {
                AnimationClip oldClip = convertClips[c];
                EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(oldClip);
                for (int b = bindings.Length - 1; b >= 0; b--)
                {
                    EditorCurveBinding oldBinding = bindings[b];
                    EditorCurveBinding newBinding = new EditorCurveBinding
                    {
                        propertyName = oldBinding.propertyName.Replace(from, to),
                        type = typeof(SkinnedMeshRenderer),
                        path = oldBinding.path
                    };
                    AnimationCurve oldCurve = AnimationUtility.GetEditorCurve(oldClip, oldBinding);
                    AnimationCurve newCurve = new AnimationCurve();
                    for (int i = 0; i < oldCurve.length; i++)
                    {
                        Keyframe oldKey = oldCurve.keys[i];
                        Keyframe newKey = new Keyframe(oldKey.time, oldKey.value, oldKey.inTangent, oldKey.outTangent, oldKey.inWeight, oldKey.outWeight);
                        newCurve.AddKey(newKey);
                        AnimationUtility.SetKeyLeftTangentMode(newCurve, newCurve.length - 1, AnimationUtility.GetKeyLeftTangentMode(oldCurve, i));
                        AnimationUtility.SetKeyRightTangentMode(newCurve, newCurve.length - 1, AnimationUtility.GetKeyRightTangentMode(oldCurve, i));
                    }
                    AnimationUtility.SetEditorCurve(oldClip, newBinding, newCurve);
                    AnimationUtility.SetEditorCurve(oldClip, oldBinding, null);
                }
            }
        }

#if ODIN_INSPECTOR
        [InlineEditor]
#endif
        public FacePropertyBindings bindings;
        public Dictionary<string, string> blendshapePairings;

        [Button]
        public void ConvertMotionCaptureToRawBlendAnimation()
        {
            if (bindings == null) return;
            blendshapePairings = new Dictionary<string, string>();
            foreach (SerializeableBinding serializeableBinding in bindings.allBindables)
            {
                blendshapePairings.Add(serializeableBinding.propertyName, serializeableBinding.coorespondingProperty);
            }
            animator = GetComponent<Animator>();
            for (int c = convertClips.Count - 1; c >= 0; c--)
            {
                AnimationClip oldClip = convertClips[c];
                EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(oldClip);
                for (int b = bindings.Length - 1; b >= 0; b--)
                {
                    EditorCurveBinding oldBinding = bindings[b];
                    if (!blendshapePairings.TryGetValue(oldBinding.propertyName, out string targetProperty)) continue;
                    EditorCurveBinding newBinding = new EditorCurveBinding
                    {
                        propertyName = targetProperty,
                        type = typeof(SkinnedMeshRenderer),
                        path = oldBinding.path
                    };
                    AnimationCurve oldCurve = AnimationUtility.GetEditorCurve(oldClip, oldBinding);
                    AnimationCurve newCurve = new AnimationCurve();
                    for (int i = 0; i < oldCurve.length; i++)
                    {
                        Keyframe oldKey = oldCurve.keys[i];
                        Keyframe newKey = new Keyframe(oldKey.time, oldKey.value * 100f, oldKey.inTangent, oldKey.outTangent, oldKey.inWeight, oldKey.outWeight);
                        newCurve.AddKey(newKey);
                        AnimationUtility.SetKeyLeftTangentMode(newCurve, newCurve.length - 1, AnimationUtility.GetKeyLeftTangentMode(oldCurve, i));
                        AnimationUtility.SetKeyRightTangentMode(newCurve, newCurve.length - 1, AnimationUtility.GetKeyRightTangentMode(oldCurve, i));
                    }
                    AnimationUtility.SetEditorCurve(oldClip, newBinding, newCurve);
                    AnimationUtility.SetEditorCurve(oldClip, oldBinding, null);
                }
            }
        }
#endif
    }
}
