using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine.Rendering;
#endif

namespace ThunderRoad
{
    public static class Extensions
    {
#if UNITY_EDITOR
	    //ShaderVariantCollection extensions, because unity sucks and makes us do this
	    public static SerializedProperty GetShaders(this ShaderVariantCollection svc)
	    {
		    var serializedObject = new SerializedObject(svc);
		    SerializedProperty serializedProperty = serializedObject.FindProperty("m_Shaders");
		    return serializedProperty;
	    }

	    public static IEnumerable<Tuple<Shader, PassType, string>> GetAllVariants(this ShaderVariantCollection svc)
	    {
		    var serializedObject = new SerializedObject(svc);
		    SerializedProperty m_Shaders = serializedObject.FindProperty("m_Shaders");
		    for (int shaderIndex = 0; shaderIndex < m_Shaders.arraySize; shaderIndex++)
		    {
			    SerializedProperty collectionArrayElement = m_Shaders.GetArrayElementAtIndex(shaderIndex);
			    //get the shader
			    Shader shader = (Shader) collectionArrayElement.FindPropertyRelative("first").objectReferenceValue;
			    //check if the shader is null
			    if (shader == null)
			    { 
				    Debug.LogWarning($"Shader is null at index {shaderIndex} in shader variant collection {svc.name}");
				    continue;
			    }

			    //get the variants array
			    SerializedProperty variantArray = collectionArrayElement.FindPropertyRelative("second.variants");
			    for (int index = 0; index < variantArray.arraySize; index++)
			    {
				    SerializedProperty variantArrayElement = variantArray.GetArrayElementAtIndex(index);
				    if(variantArrayElement == null)
				    {
					    Debug.LogWarning($"Variant is null at index {index} for shader {shader.name} in shader variant collection {svc.name}");
					    continue;
				    }
				    string keywords = variantArrayElement.FindPropertyRelative("keywords").stringValue;
				    PassType passType = (PassType) variantArrayElement.FindPropertyRelative("passType").intValue;
				    yield return new Tuple<Shader, PassType, string>(shader, passType, keywords);
			    }
		    }
	    }

	    //Svc > shader > shader index
	    public static Dictionary<ShaderVariantCollection, Dictionary<Shader, int>> svcCache;
	    public static bool AddShader(this ShaderVariantCollection svc, Shader shader, PassType passType, string keywords)
	    {
		    Profiler.BeginSample("MergeVariants");
		    if(svcCache == null) svcCache = new Dictionary<ShaderVariantCollection, Dictionary<Shader, int>>();
		    //add the svc if it doesnt exist
		    if(!svcCache.TryGetValue(svc, out var shaderIndexCache))
		    {
			    shaderIndexCache = new Dictionary<Shader, int>();
			    svcCache.Add(svc, shaderIndexCache);
		    }
		    
		    var serializedObject = new SerializedObject(svc);
		    SerializedProperty m_Shaders = serializedObject.FindProperty("m_Shaders");
		    //check if the shader is already in the collection
		    SerializedProperty pairProperty = null;
		    SerializedProperty shaderProperty = null;
		    SerializedProperty variantProperty = null;
		    //check if its already in the cache
		    if(shaderIndexCache.TryGetValue(shader, out int cachedIndex))
		    {
			    pairProperty = m_Shaders.GetArrayElementAtIndex(cachedIndex);
			    shaderProperty = pairProperty.FindPropertyRelative("first");
			    variantProperty = pairProperty.FindPropertyRelative("second.variants");
		    }
		    else
		    {
			    // look through the collection for the shader, add it to the cache when found
			    for (int shaderIndex = 0; shaderIndex < m_Shaders.arraySize; shaderIndex++)
			    {
				    pairProperty = m_Shaders.GetArrayElementAtIndex(shaderIndex); //this is expensive
				    var foundShader = pairProperty.FindPropertyRelative("first");
				    if (foundShader.objectReferenceValue == shader)
				    {
					    shaderIndexCache.Add(shader, shaderIndex);
					    shaderProperty = foundShader;
					    variantProperty = pairProperty.FindPropertyRelative("second.variants");
					    break;
				    }
			    }
		    }
		    //Otherwise add it
		    if (shaderProperty == null)
		    {
			    //add this shader to the property
			    int index = m_Shaders.arraySize;
			    m_Shaders.InsertArrayElementAtIndex(index);
			    shaderIndexCache.Add(shader, index); // add to the cache
			    pairProperty = m_Shaders.GetArrayElementAtIndex(index);
			    shaderProperty = pairProperty.FindPropertyRelative("first");
			    shaderProperty.objectReferenceValue = shader;
			    variantProperty = pairProperty.FindPropertyRelative("second.variants");
			    //clear the variants
			    variantProperty.ClearArray();
		    }
		    if (variantProperty != null)
		    {
			    //insert the variant
			    int index = variantProperty.arraySize;
			    variantProperty.InsertArrayElementAtIndex(index);
			    var variant = variantProperty.GetArrayElementAtIndex(index);
			    variant.FindPropertyRelative("passType").intValue = (int) passType;
			    variant.FindPropertyRelative("keywords").stringValue = keywords;
			    if (serializedObject.ApplyModifiedPropertiesWithoutUndo())
			    {
				    Profiler.EndSample();
				    return true;
			    }
		    }
		    Profiler.EndSample();
		    return false;
		    
	    }
	    
	    
        public static void RemoveMissingAddressableReferences(this AddressableAssetSettings settings)
        {
            bool modified = false;
            List<int> missingGroupsIndices = new List<int>();
            for (int i = 0; i < settings.groups.Count; i++)
            {
                var g = settings.groups[i];
                if (g == null)
                    missingGroupsIndices.Add(i);
            }
            if (missingGroupsIndices.Count > 0)
            {
                Debug.Log($"Addressable settings contains {missingGroupsIndices.Count} group reference(s) that are no longer there. Removing reference(s).");
                for (int i = missingGroupsIndices.Count - 1; i >= 0; i--)
                {
                    settings.groups.RemoveAt(missingGroupsIndices[i]);
                }
                modified = true;
            }
            if (modified)
            {
                settings.SetDirty(AddressableAssetSettings.ModificationEvent.GroupRemoved, null, true, true);
            }
        }
#endif
        public static void PlayFade(this AudioSource audioSource, ref Coroutine coroutine, MonoBehaviour coroutineOwner, float delay, bool oneShot = false, AudioClip audioClip = null)
        {
            if (coroutine != null)
            {
                coroutineOwner.StopCoroutine(coroutine);
            }

            if (oneShot)
            {
                audioSource.PlayOneShot(audioClip ? audioClip : audioSource.clip);
            }
            else
            {
                audioSource.Play();
            }
 
            float currentVolume = audioSource.volume;
            coroutine = coroutineOwner.ProgressiveAction(delay, (t) => audioSource.volume = Mathf.Lerp(currentVolume, 1, t));
        }

        public static void StopFade(this AudioSource audioSource, ref Coroutine coroutine, MonoBehaviour coroutineOwner, float delay)
        {
            if (coroutine != null)
            {
                coroutineOwner.StopCoroutine(coroutine);
            }
            float currentVolume = audioSource.volume;
            coroutine = coroutineOwner.ProgressiveAction(delay, (t) => { audioSource.volume = Mathf.Lerp(currentVolume, 0, t); if (t == 1) audioSource.Stop(); });
        }

        public static Coroutine DelayedAction(this MonoBehaviour monoBehaviour, float delay, Action delayedAction)
        {
            return monoBehaviour.StartCoroutine(DelayedActionCoroutine(delay, delayedAction));
        }

        static IEnumerator DelayedActionCoroutine(float delay, Action delayedAction)
        {
            yield return new WaitForSeconds(delay);
            delayedAction.Invoke();
        }

        public static Coroutine ProgressiveAction(this MonoBehaviour monoBehaviour, float delay, Action<float> progressiveAction)
        {
            return monoBehaviour.StartCoroutine(ProgressiveActionCoroutine(delay, progressiveAction));
        }

        static IEnumerator ProgressiveActionCoroutine(float delay, Action<float> progressiveAction)
        {
            float time = 0;
            while (time < delay)
            {
                progressiveAction.Invoke(time / delay);
                time += Time.deltaTime;
                yield return null;
            }
            progressiveAction.Invoke(1);
        }

        public static string GetPathFrom(this Transform transform, Transform root)
        {
            if (transform == root) return null;
            string path = "/" + transform.name;
            while (transform.parent != null)
            {
                if (transform.parent == root) break;
                transform = transform.parent;
                path = "/" + transform.name + path;
            }
            path = path.Remove(0, 1);
            return path;
        }

        public static bool TryGetCustomAttribute<T>(this MemberInfo memberInfo, out T attribute) where T : Attribute
        {
            attribute = memberInfo.GetCustomAttribute<T>();
            return attribute != null;
        }
        
        public static bool TryGetCount(this ICollection collection, out int count)
        {
            count = 0;
            if (collection == null) return false;
            count = collection.Count;
            return true;
        }

        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component) where T : Component
        {
            component = gameObject.GetComponentInChildren<T>();
            return component;
        }

        public static bool TryGetOrAddComponentInChildren<T>(this GameObject gameObject, out T component) where T : Component
        {
            component = gameObject.GetComponentInChildren<T>();
            if (component == null)
            {   //add it to a child gameobject
                var holder = new GameObject();
                holder.transform.SetParent(gameObject.transform);
                component = holder.AddComponent<T>();
            }
            return component;
        }

        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            var component = self.GetComponent<T>();
            if (component == null)
                component = self.AddComponent<T>();
            return component;
        }
        
        public static bool TryGetOrAddComponent<T>(this GameObject gameObject, out T component) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out component))
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        public static PhysicBody GetPhysicBodyInParent(this Component component) => component.gameObject.GetPhysicBodyInParent();

        public static PhysicBody GetPhysicBodyInParent(this GameObject gameObject)
        {
            PhysicBody physicBody = gameObject.GetPhysicBody();
            if (physicBody != null)
            {
                return physicBody;
            }
            Transform t = gameObject.transform;
            while (t.parent is not null)
            {
                physicBody = t.parent.gameObject.GetPhysicBody();
                if (physicBody != null)
                {
                    return physicBody;
                }
                t = t.parent.transform;
            }
            return null;
        }

        public static PhysicBody[] GetPhysicBodiesInChildren(this Component component, bool includeInactive = false) => component.gameObject.GetPhysicBodiesInChildren(includeInactive);

        public static PhysicBody[] GetPhysicBodiesInChildren(this GameObject gameObject, bool includeInactive = false)
        {
            var rbs = gameObject.GetComponentsInChildren<Rigidbody>(includeInactive);
            int rbsLength = rbs.Length;
            var abs = gameObject.GetComponentsInChildren<ArticulationBody>(includeInactive);
            PhysicBody[] pbs = new PhysicBody[rbsLength + abs.Length];
            for (int i = 0; i < pbs.Length; i++)
            {
                if (i < rbsLength) pbs[i] = rbs[i].GetPhysicBody();
                else pbs[i] = abs[i - rbsLength].GetPhysicBody();
            }
            return pbs;
        }

        public static bool IsPhysicBody(this RaycastHit raycastHit, PhysicBody physicBody)
        {
            return physicBody.rigidBody == raycastHit.rigidbody;
        }

        public static bool TryGetPhysicBody(this Collider collider, out PhysicBody physicBody)
        {
            if (collider.attachedRigidbody)
            {
                physicBody = new PhysicBody(collider.attachedRigidbody);
                return true;
            }
            physicBody = null;
            return false;
        }

        public static PhysicBody AsPhysicBody(this Rigidbody rb) => new PhysicBody(rb);
        
        public static PhysicBody GetPhysicBody(this RaycastHit hit)
        {
            if (hit.rigidbody != null) return hit.rigidbody.AsPhysicBody();
            return hit.transform.GetPhysicBody();
        }

        public static PhysicBody GetPhysicBody(this Component component)
        {
            return component switch {
                Rigidbody rb => new PhysicBody(rb),
                Collider col when col.attachedRigidbody != null => col.attachedRigidbody.AsPhysicBody(),
                _ => component.gameObject.GetPhysicBody()
            };
        }

        public static PhysicBody GetPhysicBody(this GameObject gameObject)
        {
            return gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidBody) ? new PhysicBody(rigidBody) : null;
        }

        public static void SetConnectedPhysicBody(this Joint joint, PhysicBody physicBody)
        {

            joint.connectedBody = physicBody.rigidBody;
            
        }

        public static PhysicBody GetConnectedPhysicBody(this Joint joint)
        {
            if (joint.connectedArticulationBody)
            {
                return joint.connectedArticulationBody.gameObject.GetPhysicBody();
            }
            return joint.connectedBody.gameObject.GetPhysicBody();
        }

        public static bool CalculateBodyLaunchVector(this PhysicBody physicbody, Vector3 target, out Vector3 launchVector, float speed = -1f, float gravityMultiplier = 1f)
        {
            Vector3 toTarget = target - physicbody.gameObject.transform.position;
            if (speed < 0) speed = physicbody.velocity.magnitude;
            return Utils.CalculateProjectileLaunchVector(toTarget, speed, out launchVector, gravityMultiplier);
        }

        public static U FindOrAddList<T, U, V>(this Dictionary<T, U> dict, T key) where U : ICollection<V>, new()
        {
            U found;
            if (dict == null) dict = new Dictionary<T, U>();
            if (!dict.TryGetValue(key, out found))
            {
                found = new U();
                dict[key] = found;
            }
            return found;
        }

        public static void AddToKeyedList<T, U, V>(this Dictionary<T, U> dict, T key, V element) where U : ICollection<V>, new() => dict.FindOrAddList<T, U, V>(key).Add(element);

        public static void RemoveFromKeyedList<T, U, V>(this Dictionary<T, U> dict, T key, V element) where U : ICollection<V>, new() => dict.FindOrAddList<T, U, V>(key).Remove(element);

        /// <summary>
        /// This is a O(1) way of removing things from a list by moving them to the end
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="listCount"></param>
        /// <typeparam name="T"></typeparam>
        public static void RemoveAtIgnoreOrder<T>(this IList<T> list, int index, int listCount)
        {
            int last = listCount - 1;
            if (last != index)
            {
                //copy the reference of the last one to the index we are removing
                //overwriting the one we want to remove
                list[index] = list[last];
            }
            //Remove the last thing in the list since its now a duplicate reference to list[index]
            list.RemoveAt(last);
        }

        /// <summary>
        /// This is a O(1) way of removing things from a list by moving them to the end
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <typeparam name="T"></typeparam>
        public static void RemoveAtIgnoreOrder<T>(this IList<T> list, int index)
        {
            list.RemoveAtIgnoreOrder(index, list.Count);
        }


        public static List<T> Shuffle<T>(this List<T> list)
        {
            /// With the Fisher-Yates shuffle, we randomly sort elements. This is an accurate, effective shuffling method for all array types
            System.Random _random = new System.Random();
            T obj;
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                int r = i + (int)(_random.NextDouble() * (n - i));
                obj = list[r];
                list[r] = list[i];
                list[i] = obj;
            }
            return list;
        }

        public static T[] Shuffle<T>(this T[] array)
        {
            /// With the Fisher-Yates shuffle, we randomly sort elements. This is an accurate, effective shuffling method for all array types
            System.Random _random = new System.Random();
            T obj;
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                int r = i + (int)(_random.NextDouble() * (n - i));
                obj = array[r];
                array[r] = array[i];
                array[i] = obj;
            }
            return array;
        }

        public static void BlackAndWhiteSort<T>(this List<T> list, int count, Func<T, bool> validCheck, Action<T> validAction, Action<T> invalidAction) => BlackAndWhiteSort<T>(list, count, validCheck, validAction, invalidAction, out _);

        public static void BlackAndWhiteSort<T>(this List<T> list, int count, Func<T, bool> validCheck, Action<T> validAction, Action<T> invalidAction, out int validNumber)
        {
            int currentIndex = 0;
            int validIndex = 0;
            int invalidIndex = count - 1;
            // Black and white sort means all valid things get moved to the front, and all invalid things get moved to the back, we can safely say the array is sorted once the index (which only increases if nothing moves) is greater than the invalid index (which moves closer to 0 as the sort proceeds)
            while (currentIndex <= invalidIndex)
            {
                int moveIndex;
                T item = list[currentIndex];
                if (validCheck(item))
                {
                    // is valid. perform valid action (if any) then move on
                    if (validAction != null) validAction(item);
                    moveIndex = validIndex;
                    validIndex++;
                }
                else
                {
                    // is NOT valid. perform invalid action (if any) then move on
                    if (invalidAction != null) invalidAction(item);
                    moveIndex = invalidIndex;
                    invalidIndex--;
                }
                if (currentIndex != moveIndex)
                {
                    // move if needed
                    T move = list[moveIndex];
                    list[moveIndex] = list[currentIndex];
                    list[currentIndex] = move;
                }
                else
                {
                    // otherwise, move to the next thing
                    currentIndex++;
                }
            }
            validNumber = validIndex;
        }

        public static void BlackAndWhiteSort<T>(this T[] array, int count, Func<T, bool> validCheck, Action<T> validAction, Action<T> invalidAction) => BlackAndWhiteSort<T>(array, count, validCheck, validAction, invalidAction, out _);

        public static void BlackAndWhiteSort<T>(this T[] array, int count, Func<T, bool> validCheck, Action<T> validAction, Action<T> invalidAction, out int validNumber)
        {
            int currentIndex = 0;
            int validIndex = 0;
            int invalidIndex = count - 1;
            // Black and white sort means all valid things get moved to the front, and all invalid things get moved to the back, we can safely say the array is sorted once the index (which only increases if nothing moves) is greater than the invalid index (which moves closer to 0 as the sort proceeds)
            while (currentIndex <= invalidIndex)
            {
                int moveIndex;
                T item = array[currentIndex];
                if (validCheck(item))
                {
                    // is valid. perform valid action (if any) then move on
                    if (validAction != null) validAction(item);
                    moveIndex = validIndex;
                    validIndex++;
                }
                else
                {
                    // is NOT valid. perform invalid action (if any) then move on
                    if (invalidAction != null) invalidAction(item);
                    moveIndex = invalidIndex;
                    invalidIndex--;
                }
                if (currentIndex != moveIndex)
                {
                    // move if needed
                    T move = array[moveIndex];
                    array[moveIndex] = array[currentIndex];
                    array[currentIndex] = move;
                }
                else
                {
                    // otherwise, move to the next thing
                    currentIndex++;
                }
            }
            validNumber = validIndex;
        }

        public static bool RandomFilteredSelectInPlace<T>(this IEnumerable<T> enumerable, Func<T, bool> condition, out T result) => enumerable.WeightedFilteredSelectInPlace(condition, (element) => 1, out result);

        public static bool WeightedFilteredSelectInPlace<T>(this IEnumerable<T> enumerable, Func<T, bool> condition, Func<T, float> weight, out T result)
        {
            bool found = false;
            result = default(T);
            float totalWeight = 0;
            foreach (T element in enumerable)
            {
                if (condition(element))
                {
                    float elementWeight = weight(element);
                    if (elementWeight > 0f && UnityEngine.Random.Range(0f, totalWeight + elementWeight) >= totalWeight)
                    {
                        result = element;
                        found = true;
                    }
                    totalWeight += elementWeight;
                }
            }
            return found;
        }

        public static T WeightedSelect<T>(this IEnumerable<T> enumerable, Func<T, float> weight)
        {
            T result = default(T);
            float totalWeight = 0;
            foreach (T element in enumerable)
            {
                float elementWeight = weight(element);
                if (elementWeight > 0f && UnityEngine.Random.Range(0f, totalWeight + elementWeight) >= totalWeight)
                {
                    result = element;
                }
                totalWeight += elementWeight;
            }
            return result;
        }

        public static byte[] Serialize<T>(this T source)
        {
            JsonSerializer ser = Catalog.GetJsonNetSerializer();
            using (MemoryStream s = new MemoryStream())
            {
                //serialize to the stream, leave the stream open
                using (var writer = new StreamWriter(stream: s, encoding: Encoding.UTF8, bufferSize: 4096))
                using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                {
                    ser.Serialize(jsonWriter, source);
                    jsonWriter.Flush();
                }

                return s.ToArray();
            }
        }
        
        public static T Deserialize<T>(this byte[] source)
        {
            JsonSerializer ser = Catalog.GetJsonNetSerializer();
            using (MemoryStream s = new MemoryStream(source))
            {
                //deserialize from the stream
                using (StreamReader reader = new StreamReader(s))
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    T output = ser.Deserialize<T>(jsonReader);
                    s.Dispose();
                    return output;
                }
            }
        }
        
        public static async Task<T> CloneJsonAsync<T>(this T source)
        {
            return await Task.Run(() => source.CloneJson());
        }

        public static T CloneJson<T>(this T source)
        {
            JsonSerializer ser = Catalog.GetJsonNetSerializer();
            using (MemoryStream s = new MemoryStream())
            {
                //serialize to the stream, leave the stream open
                using (var writer = new StreamWriter(stream: s, encoding: Encoding.UTF8, bufferSize: 4096, leaveOpen: true))
                using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                {
                    ser.Serialize(jsonWriter, source);
                    jsonWriter.Flush();
                }
                //seek to the start of the stream
                s.Seek(0, SeekOrigin.Begin);
                //deserialize from the stream
                using (StreamReader reader = new StreamReader(s))
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    T output = ser.Deserialize<T>(jsonReader);
                    s.Dispose();
                    return output;
                }
            }
        }

        public static IEnumerator AsIEnumerator(this Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }
            if (task.IsFaulted)
            {
                throw task.Exception;
            }
        }

        /// <summary>
        /// Forces iteration through an IEnumerator, useful to make coroutines synchronous
        /// </summary>
        /// <param name="enumerator"></param>
        public static void AsSynchronous(this IEnumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is IEnumerator next)
                {
                    next.AsSynchronous();
                }
            }
        }

        public static bool IsApproximately(this float a, float b) => Mathf.Approximately(a, b);

        public static bool IsNormalized(this float f) => f >= 0f && f <= 1f;

        public static bool IsInfiniteOrNaN(this float f) => float.IsNaN(f) || float.IsInfinity(f) || float.IsNegativeInfinity(f);

        public static float Sum(this float[] floats)
        {
            float result = 0f;
            for (int i = 0; i < floats.Length; i++) result += floats[i];
            return result;
        }

        public static float DistanceSqr(this Vector3 a, Vector3 b)
        {
            return GetDistanceSqr(a, b);
        }
        
        /// <summary>
        /// Returns the Squared Distance between two vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Squared Distance</returns>
        public static float GetDistanceSqr(Vector3 a, Vector3 b)
        {
            float num1 = a.x - b.x;
            float num2 = a.y - b.y;
            float num3 = a.z - b.z;
            return num1 * num1 + num2 * num2 + num3 * num3;
        }

        public static bool ValueBetween(this Vector2Int vector, int value)
        {
            return value >= vector.x && value <= vector.y;
        }

        public static bool ValueBetween(this Vector2 vector, float value)
        {
            return value >= vector.x && value <= vector.y;
        }
        
        public static bool PointInRadius(this Vector3 vectorA, Vector3 vectorB, float radius)
        {
            // Best performance to check radius
            float sqrMagnitude = vectorA.DistanceSqr(vectorB);
            if (sqrMagnitude < radius * radius)
            {
                return true;
            }
            return false;
        }

        public static bool PointInRadius(this Vector3 vectorA, Vector3 vectorB, float radius, out float radiusDistanceRatio)
        {
            // Best performance to check radius
            float sqrMagnitude = vectorA.DistanceSqr(vectorB);
            if (sqrMagnitude < radius * radius)
            {
                radiusDistanceRatio = 1 - (sqrMagnitude / (radius * radius));
                return true;
            }
            radiusDistanceRatio = 0;
            return false;
        }

        public static (Vector3 inDirection, Vector3 notInDirection) ProjectAndSubtract(this Vector3 vector, Vector3 normal)
        {
            (Vector3 inDirection, Vector3 notInDirection) result = (Vector3.Project(vector, normal), Vector3.zero);
            result.notInDirection = vector - result.inDirection;
            return result;
        }

        public static Vector3 GetClosestPoint(this Vector3 origin, params Vector3[] checkPoints)
        {
            Vector3 closest = checkPoints[0];
            for (int i = 1; i < checkPoints.Length; i++)
            {
                if ((checkPoints[i] - origin).sqrMagnitude < (closest - origin).sqrMagnitude) closest = checkPoints[i];
            }
            return closest;
        }

        public static T GetClosestObject<T>(this Vector3 origin, params T[] objects) where T : Component
        {
            T closest = objects[0];
            for (int i = 1; i < objects.Length; i++)
            {
                if ((objects[i].transform.position - origin).sqrMagnitude < (closest.transform.position - origin).sqrMagnitude) closest = objects[i];
            }
            return closest;
        }

        public static Vector3 GetFurthestPoint(this Vector3 origin, params Vector3[] checkPoints)
        {
            Vector3 furthest = checkPoints[0];
            for (int i = 1; i < checkPoints.Length; i++)
            {
                if ((checkPoints[i] - origin).sqrMagnitude > (furthest - origin).sqrMagnitude) furthest = checkPoints[i];
            }
            return furthest;
        }

        public static T GetFurthestObject<T>(this Vector3 origin, params T[] objects) where T : Component
        {
            T furthest = objects[0];
            for (int i = 1; i < objects.Length; i++)
            {
                if ((objects[i].transform.position - origin).sqrMagnitude > (furthest.transform.position - origin).sqrMagnitude) furthest = objects[i];
            }
            return furthest;
        }

        public static bool HasFlagNoGC(this EffectModule.PlatformFilter flags, EffectModule.PlatformFilter value)
        {
            return ((flags & value) > 0);
        }

        public static bool HasFlagNoGC(this ItemData.Storage flags, ItemData.Storage value)
        {
            return ((flags & value) > 0);
        }

        public static bool HasFlagNoGC(this BuildSettings.ContentFlag flags, BuildSettings.ContentFlag value)
        {
            return ((flags & value) > 0);
        }

        public static bool CheckContentActive(this BuildSettings.ContentFlag flags, BuildSettings.ContentFlag content)
        {
            if (flags == BuildSettings.ContentFlag.None) return false;
            if (content == BuildSettings.ContentFlag.None) return true;
            return (flags & content) == content;
        }

        public static BuildSettings.ContentFlag AsRealFlag(this BuildSettings.SingleContentFlag flag)
        {
            return (BuildSettings.ContentFlag)(1 << (int)flag);
        }

        public static (Side side, ItemModuleAI.AttackType motion) SplitSideAndMotion(this AnimationData.Clip.MotionType clipMotion)
        {
            switch (clipMotion)
            {
                case AnimationData.Clip.MotionType.RightSwing: return (Side.Right, ItemModuleAI.AttackType.Swing);
                case AnimationData.Clip.MotionType.RightThrust: return (Side.Right, ItemModuleAI.AttackType.Thrust);
                case AnimationData.Clip.MotionType.LeftSwing: return (Side.Left, ItemModuleAI.AttackType.Swing);
                case AnimationData.Clip.MotionType.LeftThrust: return (Side.Left, ItemModuleAI.AttackType.Thrust);
            }
            return (Side.Right, ItemModuleAI.AttackType.None);
        }

        public static Collider Clone(this Collider collider, GameObject gameObject)
        {
            if (collider is SphereCollider) Clone(collider as SphereCollider, gameObject);
            else if (collider is CapsuleCollider) Clone(collider as CapsuleCollider, gameObject);
            else if (collider is BoxCollider) Clone(collider as BoxCollider, gameObject);
            else if (collider is MeshCollider) Clone(collider as MeshCollider, gameObject);
            return null;
        }

        public static SphereCollider Clone(this SphereCollider collider, GameObject gameObject)
        {
            SphereCollider clonedCollider = gameObject.AddComponent<SphereCollider>();
            clonedCollider.center = collider.center;
            clonedCollider.radius = collider.radius;
            clonedCollider.sharedMaterial = collider.sharedMaterial;
            clonedCollider.isTrigger = collider.isTrigger;
            return clonedCollider;
        }

        public static CapsuleCollider Clone(this CapsuleCollider collider, GameObject gameObject)
        {
            CapsuleCollider clonedCollider = gameObject.AddComponent<CapsuleCollider>();
            clonedCollider.center = collider.center;
            clonedCollider.radius = collider.radius;
            clonedCollider.height = collider.height;
            clonedCollider.sharedMaterial = collider.sharedMaterial;
            clonedCollider.isTrigger = collider.isTrigger;
            return clonedCollider;
        }

        public static BoxCollider Clone(this BoxCollider collider, GameObject gameObject)
        {
            BoxCollider clonedCollider = gameObject.AddComponent<BoxCollider>();
            clonedCollider.center = collider.center;
            clonedCollider.size = collider.size;
            clonedCollider.sharedMaterial = collider.sharedMaterial;
            clonedCollider.isTrigger = collider.isTrigger;
            return clonedCollider;
        }

        public static MeshCollider Clone(this MeshCollider collider, GameObject gameObject)
        {
            MeshCollider clonedCollider = gameObject.AddComponent<MeshCollider>();
            clonedCollider.convex = collider.convex;
            clonedCollider.cookingOptions = collider.cookingOptions;
            clonedCollider.sharedMesh = collider.sharedMesh;
            clonedCollider.sharedMaterial = collider.sharedMaterial;
            clonedCollider.isTrigger = collider.isTrigger;
            return clonedCollider;
        }

        public static float GetScaledRadius(this SphereCollider sphere) => sphere.radius * Mathf.Max(sphere.transform.lossyScale.x, sphere.transform.lossyScale.y, sphere.transform.lossyScale.z);

        public static void ScaledWorldInfo(this CapsuleCollider capsule, out Vector3 pointA, out Vector3 pointB, out float height, out float radius)
        {
            pointA = pointB = capsule.transform.TransformPoint(capsule.center);
            capsule.GetScaledHeightRadius(out height, out radius);
            float halfHeightMinusRadius = (height / 2f) - radius;
            if (halfHeightMinusRadius <= 0f) return;
            Vector3 capsuleAxis = capsule.transform.TransformDirection(capsule.AxisVector());
            pointA = capsule.transform.TransformPoint(capsule.center) + (capsuleAxis * halfHeightMinusRadius);
            pointB = capsule.transform.TransformPoint(capsule.center) + (capsuleAxis * -halfHeightMinusRadius);
        }

        public static void FocusPoints(this CapsuleCollider capsule, out Vector3 pointA, out Vector3 pointB) => capsule.ScaledWorldInfo(out pointA, out pointB, out _, out _);

        public static Vector3 AxisVector(this CapsuleCollider capsule) => new Vector3(capsule.direction == 0 ? 1f : 0f, capsule.direction == 1 ? 1f : 0f, capsule.direction == 2 ? 1f : 0f);

        public static float GetScaledRadius(this CapsuleCollider capsule)
        {
            capsule.GetScaledHeightRadius(out _, out var radius);
            return radius;
        }

        public static float GetScaledHeight(this CapsuleCollider capsule)
        {
            capsule.GetScaledHeightRadius(out var height, out _);
            return height;
        }

        public static void GetScaledHeightRadius(this CapsuleCollider capsule, out float height, out float radius)
        {
            float axisScale = 1f;
            float radiusScale = 1f;
            switch (capsule.direction)
            {
                case 0:
                    axisScale = capsule.transform.lossyScale.x;
                    radiusScale = Mathf.Max(capsule.transform.lossyScale.y, capsule.transform.lossyScale.z);
                    break;
                case 1:
                    axisScale = capsule.transform.lossyScale.y;
                    radiusScale = Mathf.Max(capsule.transform.lossyScale.x, capsule.transform.lossyScale.z);
                    break;
                case 2:
                    axisScale = capsule.transform.lossyScale.z;
                    radiusScale = Mathf.Max(capsule.transform.lossyScale.x, capsule.transform.lossyScale.y);
                    break;
            }
            radius = capsule.radius * radiusScale;
            height = capsule.height * axisScale;
        }

        public static bool DistanceTouchCheck(this Collider colliderA, Collider colliderB)
        {
            ColliderClosenessInfo(colliderA, colliderB.transform.position, out var lineA, out var aIsLine, out var aDist);
            ColliderClosenessInfo(colliderB, colliderA.transform.position, out var lineB, out var bIsLine, out var bDist);
            var pointA = lineA.a;
            var pointB = lineB.a;
            if (aIsLine && bIsLine) Utils.ClosestPointsOnTwoLines(lineA.a, lineA.b, lineB.a, lineB.b, out pointA, out pointB);
            if (aIsLine && !bIsLine) pointA = Utils.ClosestPointOnLine(lineA.a, lineA.b, pointB);
            if (!aIsLine && bIsLine) pointB = Utils.ClosestPointOnLine(lineB.a, lineB.b, pointA);
            return pointA.PointInRadius(pointB, aDist + bDist);
        }

        private static void ColliderClosenessInfo(Collider col, Vector3 otherCenter, out (Vector3 a, Vector3 b) line, out bool needLine, out float distance)
        {
            line = (Vector3.zero, Vector3.zero);
            distance = 0f;
            needLine = false;
            if (col is CapsuleCollider capsule)
            {
                capsule.ScaledWorldInfo(out line.a, out line.b, out _, out distance);
                needLine = true;
            }
            else if (col is SphereCollider sphere)
            {
                line.a = line.b = sphere.transform.TransformPoint(sphere.center);
                distance = sphere.GetScaledRadius();
            }
            else if (col is BoxCollider box)
            {
                line.a = line.b = box.transform.TransformPoint(box.center);
                // if we really wanted to I'm sure we could get the true distance, but a rough distance is fine. this method should be best suited for round colliders
                distance = Vector3.Distance(box.transform.position, box.ClosestPoint(otherCenter));
            }
        }

        public static float GetFirstValue(this AnimationCurve animationCurve)
        {
            return (animationCurve.length == 0) ? 0 : animationCurve[0].value;
        }

        public static float GetLastValue(this AnimationCurve animationCurve)
        {
            return (animationCurve.length == 0) ? 0 : animationCurve[animationCurve.length - 1].value;
        }

        public static float GetFirstTime(this AnimationCurve animationCurve)
        {
            return (animationCurve.length == 0) ? 0 : animationCurve[0].time;
        }

        public static float GetLastTime(this AnimationCurve animationCurve)
        {
            return (animationCurve.length == 0) ? 0 : animationCurve[animationCurve.length - 1].time;
        }

        public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
        }

        public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 2)
        {
            float multiplier = 1;
            for (int i = 0; i < decimalPlaces; i++)
            {
                multiplier *= 10f;
            }
            return new Vector3(Mathf.Round(vector3.x * multiplier) / multiplier, Mathf.Round(vector3.y * multiplier) / multiplier, Mathf.Round(vector3.z * multiplier) / multiplier);
        }

        public static float SignedAngleFromDirection(this Vector3 fromdir, Vector3 todir, Vector3 referenceup)
        {
            // calculates the the angle between two direction vectors, with a referenceup a sign in which direction it points can be calculated (clockwise is positive and counter clockwise is negative)
            Vector3 planenormal = Vector3.Cross(fromdir, todir); // calculate the planenormal (perpendicular vector)
            float angle = Vector3.Angle(fromdir, todir); // calculate the angle between the 2 direction vectors (note: its always the smaller one smaller than 180°)
            float orientationdot = Vector3.Dot(planenormal, referenceup); // calculate wether the normal and the referenceup point in the same direction (>0) or not (<0), http://docs.unity3d.com/Documentation/Manual/ComputingNormalPerpendicularVector.html
            if (orientationdot > 0.0f) // the angle is positive (clockwise orientation seen from referenceup)
                return angle;
            return -angle; // the angle is negative (counter-clockwise orientation seen from referenceup)
        }

        public static Vector3 ToXZ(this Vector3 fromdir)
        {
            fromdir.y = 0;
            return fromdir;
        }

        public static Vector3 ToYZ(this Vector3 fromdir)
        {
            fromdir.x = 0;
            return fromdir;
        }

        public static Vector3 ClampMagnitude(this Vector3 vector, float minMagnitude, float maxMagnitude)
        {
            if (vector.sqrMagnitude < minMagnitude * minMagnitude) return vector.normalized * minMagnitude;
            if (vector.sqrMagnitude > maxMagnitude * maxMagnitude) return vector.normalized * maxMagnitude;
            return vector;
        }

        public static Transform FindOrAddTransform(this Transform parent, string name, Vector3 position, Quaternion? rotation = null, Vector3? scale = null)
        {
            Transform returnable = parent.Find(name);
            if (!returnable)
            {
                returnable = new GameObject(name).transform;
                returnable.parent = parent;
                returnable.position = position;
                returnable.rotation = rotation != null ? rotation.Value : Quaternion.identity;
                returnable.localScale = scale != null ? scale.Value : Vector3.one;
            }
            return returnable;
        }

        public static Quaternion To(this Quaternion from, Quaternion to)
        {
            return to * Quaternion.Inverse(from);
        }

        /// <summary>
        /// Sets a joint's targetRotation to match a given local rotation.
        /// The joint transform's local rotation must be cached on Start and passed into this method.
        /// </summary>
        public static void SetTargetRotationLocal(this ConfigurableJoint joint, Quaternion targetLocalRotation, Quaternion startLocalRotation)
        {
            if (joint.configuredInWorldSpace)
            {
                Debug.LogError("SetTargetRotationLocal should not be used with joints that are configured in world space. For world space joints, use SetTargetRotation.", joint);
            }
            SetTargetRotationInternal(joint, targetLocalRotation, startLocalRotation, Space.Self);
        }

        /// <summary>
        /// Sets a joint's targetRotation to match a given world rotation.
        /// The joint transform's world rotation must be cached on Start and passed into this method.
        /// </summary>
        public static void SetTargetRotation(this ConfigurableJoint joint, Quaternion targetWorldRotation, Quaternion startWorldRotation)
        {
            if (!joint.configuredInWorldSpace)
            {
                Debug.LogError("SetTargetRotation must be used with joints that are configured in world space. For local space joints, use SetTargetRotationLocal.", joint);
            }
            SetTargetRotationInternal(joint, targetWorldRotation, startWorldRotation, Space.World);
        }

        static void SetTargetRotationInternal(ConfigurableJoint joint, Quaternion targetRotation, Quaternion startRotation, Space space)
        {
            // Calculate the rotation expressed by the joint's axis and secondary axis
            var right = joint.axis;
            var forward = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
            var up = Vector3.Cross(forward, right).normalized;
            Quaternion worldToJointSpace = Quaternion.LookRotation(forward, up);

            // Transform into world space
            Quaternion resultRotation = Quaternion.Inverse(worldToJointSpace);

            // Counter-rotate and apply the new local rotation.
            // Joint space is the inverse of world space, so we need to invert our value
            if (space == Space.World)
            {
                resultRotation *= startRotation * Quaternion.Inverse(targetRotation);
            }
            else
            {
                resultRotation *= Quaternion.Inverse(targetRotation) * startRotation;
            }

            // Transform back into joint space
            resultRotation *= worldToJointSpace;

            // Set target rotation to our newly calculated rotation
            joint.targetRotation = resultRotation;
        }

        public static void SetAnimatorDefault(this Animator animator, bool keepState = true)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                // Skip any parameters controlled by curves
                if (animator.IsParameterControlledByCurve(param.nameHash)) continue;
                switch (param.type)
                {
                    case AnimatorControllerParameterType.Trigger:
                        animator.ResetTrigger(param.nameHash);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        animator.SetBool(param.nameHash, param.defaultBool);
                        break;
                    case AnimatorControllerParameterType.Int:
                        animator.SetInteger(param.nameHash, param.defaultInt);
                        break;
                    case AnimatorControllerParameterType.Float:
                        animator.SetFloat(param.nameHash, param.defaultFloat);
                        break;
                }
            }
            animator.Update(0f);
            animator.keepAnimatorStateOnDisable = keepState;
        }
        
        public static bool IsStatic(this MemberInfo member)
        {
            if (member is null) return false;
            switch (member)
            {
                case FieldInfo fieldInfo:
                    return fieldInfo.IsStatic;
                case PropertyInfo propertyInfo:
                    return !propertyInfo.CanRead ? propertyInfo.GetSetMethod(true).IsStatic : propertyInfo.GetGetMethod(true).IsStatic;
                case MethodBase methodBase:
                    return methodBase.IsStatic;
                case EventInfo eventInfo:
                    return eventInfo.GetRaiseMethod(true).IsStatic;
                case Type type:
                    return type.IsSealed && type.IsAbstract;
                default:
                    throw new NotSupportedException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Unable to determine IsStatic for member {0}.{1}MemberType was {2} but only fields, properties, methods, events and types are supported.", (object) member.DeclaringType.FullName, (object) member.Name, (object) member.GetType().FullName));
            }
        }

        public static bool IsSameOrSubclassOf(this Type type, Type baseType)
        {
            return type == baseType || type.IsSubclassOf(baseType);
        }
    }
}
