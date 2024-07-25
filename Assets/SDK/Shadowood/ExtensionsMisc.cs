using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

public static class ExtensionsMisc
{
    // https://discussions.unity.com/t/test-to-see-if-a-vector3-point-is-within-a-boxcollider/17385/5
    
    /// <summary>
    /// Return true if the point is inside the given BoxCollider.
    /// </summary>
    public static bool IsInside(this Vector3 p_Point, BoxCollider p_Box)
    {
        p_Point = p_Box.transform.InverseTransformPoint(p_Point) - p_Box.center;

        float l_HalfX = (p_Box.size.x * 0.5f);
        float l_HalfY = (p_Box.size.y * 0.5f);
        float l_HalfZ = (p_Box.size.z * 0.5f);

        return (p_Point.x < l_HalfX && p_Point.x > -l_HalfX &&
                p_Point.y < l_HalfY && p_Point.y > -l_HalfY &&
                p_Point.z < l_HalfZ && p_Point.z > -l_HalfZ);
    }
    
    public static bool IsInside(this BoxCollider p_Box, Vector3 p_Point)
    {
        p_Point = p_Box.transform.InverseTransformPoint(p_Point) - p_Box.center;

        float l_HalfX = (p_Box.size.x * 0.5f);
        float l_HalfY = (p_Box.size.y * 0.5f);
        float l_HalfZ = (p_Box.size.z * 0.5f);

        return (p_Point.x < l_HalfX && p_Point.x > -l_HalfX &&
                p_Point.y < l_HalfY && p_Point.y > -l_HalfY &&
                p_Point.z < l_HalfZ && p_Point.z > -l_HalfZ);
    }

    /// <summary>
    /// Return true if the point is inside the given SphereCollider.
    /// </summary>
    public static bool IsInside(this Vector3 p_Point, SphereCollider p_Sphere)
    {
        p_Point = p_Sphere.transform.InverseTransformPoint(p_Point) - p_Sphere.center;
        return p_Point.sqrMagnitude <= p_Sphere.radius * p_Sphere.radius;
    }
    
    public static bool IsInside(this SphereCollider p_Sphere, Vector3 p_Point)
    {
        p_Point = p_Sphere.transform.InverseTransformPoint(p_Point) - p_Sphere.center;
        return p_Point.sqrMagnitude <= p_Sphere.radius * p_Sphere.radius;
    }
    
    //public static T GetOrAddComponent<T>(this Component component) where T : Component => component.GetComponent<T>() ?? component.gameObject.AddComponent<T>();
   
    public static T GetOrAddComponent<T>(this GameObject go) where T: Component
    {
        var co = go.GetComponent<T>();
        return co ? co : go.AddComponent<T>();
    }
    public static T[] Slice<T>(this T[] source, int index, int length)
    {
        var end = Mathf.Min(length + index, source.Length);
        length = Mathf.Min(length, end - index);
        T[] slice = new T[length];
        Array.Copy(source, index, slice, 0, length);
        return slice;
    }

    public static T[] SliceyDicey<T>(this T[] source, int index, int length, int maxLength)
    {
        T[] slice = new T[maxLength];
        Array.Copy(source, index, slice, 0, length);
        return slice;
    }

    public static Bounds Transform(this Bounds bounds, Matrix4x4 matrix)
    {
        var center = matrix.MultiplyPoint(bounds.center);
        var extents = bounds.extents;
        extents.x = Mathf.Abs(extents.x * matrix.m00) + Mathf.Abs(extents.y * matrix.m01) + Mathf.Abs(extents.z * matrix.m02);
        extents.y = Mathf.Abs(extents.x * matrix.m10) + Mathf.Abs(extents.y * matrix.m11) + Mathf.Abs(extents.z * matrix.m12);
        extents.z = Mathf.Abs(extents.x * matrix.m20) + Mathf.Abs(extents.y * matrix.m21) + Mathf.Abs(extents.z * matrix.m22);

        return new Bounds(center, extents);
    }


#if UNITY_EDITOR

    public class MaterialEditorHack
    {
        public MaterialEditor materialEditor;
        public Material lastMaterial;
        
        
    }
    public static void DrawMaterialEditor(Material material, UnityEditor.SerializedObject serializedObject, ref UnityEditor.MaterialEditor materialEditor)
    {
        //RaycastTexture _target = this;
        //UnityEditor.MaterialEditor _materialEditor = null;

        //Material oldMaterial = null;

        //_target = (RaycastTexture)target;

        if (material == null) return;

        // Create an instance of the default MaterialEditor.
        if (materialEditor == null)
        {
            Debug.Log("Making editor");
            materialEditor = (UnityEditor.MaterialEditor) UnityEditor.Editor.CreateEditor(material);
        }
/*
        oldMaterial = (Material) materialEditor.target;

        if (oldMaterial != material)
        {
            Debug.Log("Changed mat");
            serializedObject.ApplyModifiedProperties();

            Object.DestroyImmediate(materialEditor); // Free the memory used by the previous MaterialEditor.
            materialEditor = (UnityEditor.MaterialEditor) UnityEditor.Editor.CreateEditor(material); // Create a new instance of the default MaterialEditor.
        }
*/
        materialEditor.DrawHeader(); // Draw the material's foldout and the material shader field. Required to call OnInspectorGUI.

        bool isDefaultMaterial = !UnityEditor.AssetDatabase.GetAssetPath(material).StartsWith("Assets"); // We need to prevent the user from editing Unity's default materials.

        using (new UnityEditor.EditorGUI.DisabledGroupScope(isDefaultMaterial))
        {
            materialEditor.OnInspectorGUI(); // Draw the material properties. Works only if the foldout of DrawHeader is open.
        }
    }
#endif
    /// <summary>
    /// var lstDst = lst.DistinctBy(item => item.Key);
    /// </summary>
    public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
    {
        return enumerable.GroupBy(keySelector).Select(grp => grp.First());
    }

    // https://gist.github.com/Arakade/9dd844c2f9c10e97e3d0
    public static void DrawString(string text, Vector3 worldPosition, Color textColor, Vector2 anchor, float textSize = 15f)
    {
#if UNITY_EDITOR
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        if (!view)
            return;
        Vector3 screenPosition = view.camera.WorldToScreenPoint(worldPosition);
        if (screenPosition.y < 0 || screenPosition.y > view.camera.pixelHeight || screenPosition.x < 0 || screenPosition.x > view.camera.pixelWidth || screenPosition.z < 0)
            return;
        var pixelRatio = UnityEditor.HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.right).x - UnityEditor.HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.zero).x;
        UnityEditor.Handles.BeginGUI();
        var style = new GUIStyle(GUI.skin.label)
        {
            fontSize = (int) textSize,
            normal = new GUIStyleState() {textColor = textColor}
        };
        Vector2 size = style.CalcSize(new GUIContent(text)) * pixelRatio;
        var alignedPosition =
            ((Vector2) screenPosition +
             size * ((anchor + Vector2.left + Vector2.up) / 2f)) * (Vector2.right + Vector2.down) +
            Vector2.up * view.camera.pixelHeight;
        GUI.Label(new Rect(alignedPosition / pixelRatio, size / pixelRatio), text, style);
        UnityEditor.Handles.EndGUI();
#endif
    }

    public static Vector3 RGB(this Color colorIn)
    {
        return new Vector3(colorIn.r, colorIn.g, colorIn.b);
    }

    public static Vector4 RGBandA(this Color colorIn, float alphaIn)
    {
        return new Vector4(colorIn.r, colorIn.g, colorIn.b, alphaIn);
    }

    public static int test;
    // UnityEvents

    public static int Modulus(this int k, int n)
    {
        int remainder = k % n;
        return (remainder < 0) ? remainder + n : remainder;
    }

    public static int Wrap(this ref int current, int max, int addition = 0)
    {
        current += addition;
        return current = ((current % max) + max) % max;
    }

    public static int Increment(this ref int current, int max)
    {
        return current = ((++current % max) + max) % max;
    }

    public static int Decrement(this ref int current, int max)
    {
        return current = ((--current % max) + max) % max;
    }

    public static float Wrap(this ref float current, float max, float addition = 0)
    {
        current += addition;
        return current = ((current % max) + max) % max;
    }

    public static float Increment(this ref float current, float max)
    {
        return current = ((++current % max) + max) % max;
    }

    public static float Decrement(this ref float current, float max)
    {
        return current = ((--current % max) + max) % max;
    }

    /// <summary>
    /// Hacky way to hash a UnityEvent
    /// </summary>
    public static int GetHashCodeHack<T>(this UnityEvent<T> unityEvent)
    {
        var count = unityEvent.GetPersistentEventCount();
        int hash = count;
        for (int i = 0; i < count; i++)
        {
            var mname = unityEvent.GetPersistentMethodName(i);
            var target = unityEvent.GetPersistentTarget(i);
            hash += mname.GetHashCode();
            hash += target.GetHashCode();
        }

        return hash;
    }

    ///
    /// Gets all the actions in the event as an IEnumerable(UnityAction).
    ///
    public static IEnumerable GetAllActionsAsEnumerable<T>(this UnityEvent<T> @event)
    {
        // Loop through all of the actions in the event.
        for (int i = 0; i < @event.GetPersistentEventCount(); i++)
        {
            // Get the information about the action.
            MethodInfo actionInfo = UnityEventBase.GetValidMethodInfo(@event.GetPersistentTarget(i), @event.GetPersistentMethodName(i), new Type[0]);
            // Cast actionInfo into a UnityAction to get the listener.
            yield return actionInfo.Invoke(@event.GetPersistentTarget(i), null);
        }
    }

    ///
    /// Gets all the actions in the event as an array of UnityAction.
    ///
    public static UnityAction<T>[] GetAllActions<T>(this UnityEvent<T> @event)
    {
        // Make a list of the actions that will be returned at the end.
        var actions = new List<UnityAction<T>>();
        // Get all the actions as an enumerable.
        IEnumerable eventActions = @event.GetAllActionsAsEnumerable();
        // Loop through the actions and add them to the actions list.
        foreach (UnityAction<T> currentAction in eventActions)
            actions.Add(currentAction);
        // Return the list as an array.
        return actions.ToArray();
    }

    ///
    /// Gets a specific action at the index of index.
    ///
    public static UnityAction<T> GetAction<T>(this UnityEvent<T> @event, int index)
    {
        return @event.GetAllActions<T>()[index];
    }


    /*
	public static void SetData<T>( this BoxedArray<T> boxedArray, T[] dataIn, int managedDataInIndex = 0, int computeBufferIndex = 0, int count = -1 ) {
		if( count <= -1 ) count = dataIn.Length;
		for( int i = 0; i < count; i++ ) {
			boxedArray.data[i + managedDataInIndex] = dataIn[i + computeBufferIndex];
		}
	}

	public static void SetData<T>( this BoxedArray<T> boxedArray, T[] dataIn ) {
		boxedArray.data = dataIn;
	}
	*/
    //public static T[] SetData<T>( this T[] array, T[] dataIn ) {
    //	return dataIn;
    //}


    public static T[] SetData<T>(this T[] array, T[] dataIn, int managedDataInIndex = 0, int computeBufferIndex = 0, int count = -1)
    {
        if (count <= -1) count = dataIn.Length;

        if (count + computeBufferIndex > array.Length) Debug.LogError("IndexB error:" + count + "+" + (computeBufferIndex) + ":" + array.Length);
        if (count + managedDataInIndex > dataIn.Length) Debug.LogError("IndexA error:" + count + "+" + (managedDataInIndex) + ":" + dataIn.Length);

        for (int i = 0; i < count; i++)
        {
            array[i + computeBufferIndex] = dataIn[i + managedDataInIndex];
        }

        return array;
    }

    /*
	public static T[] GetData<T>( this BoxedArray<T> boxedArray ) {
		return boxedArray.data;
	}

	public static T[] GetData<T>( this T[] array ) {
		return array;
	}

	public static T[] GetData<T>( this BoxedArray<T> boxedArray, int startIndexA = 0, int startIndexB = 0, int count = -1 ) {
		if( count <= -1 ) count = boxedArray.data.Length;

		var result = new T[count];

		for( int i = 0; i < count; i++ ) {
			result[i + startIndexB] = boxedArray.data[i + startIndexA];
		}
		return result;
	}*/

    public static T[] GetData<T>(this T[] array, int managedDataInIndex = 0, int computeBufferIndex = 0, int count = -1)
    {
        if (count <= -1) count = array.Length;

        var result = new T[count];

        for (int i = 0; i < count; i++)
        {
            result[i + computeBufferIndex] = array[i + managedDataInIndex];
        }

        return result;
    }

    /*
	public static bool ComputeIsEmpty<T>( this BoxedArray<T> list, int index = 0 ) {
		if( list == null ) return true;
		if( index > 0 ) {
			if( list.data.Length != index ) return true;
		} else {
			if( list.data.Length == 0 ) return true;
		}
		return false;
	}

	public static void Dispose<T>( this BoxedArray<T> list ) {

		for( int i = 0; i < list.data.Length; i++ ) {
			list.data[i] = default( T );
		}
		list.data = new T[0];

		list.data = null;
	}*/


    public static int NextIndex<T>(this List<T> list, int index)
    {
        int nextindex = 0;
        if (index == list.Count - 1)
        {
            // is last item
            nextindex = 0;
        }
        else
        {
            nextindex = index + 1;
        }

        return nextindex;
    }

    public static T NextItem<T>(this List<T> list, int index)
    {
        return list[list.NextIndex(index)];
    }

    public static int PrevIndex<T>(this List<T> list, int index)
    {
        int prevIndex = 0;
        if (index == 0)
        {
            // is last item
            prevIndex = list.Count - 1;
        }
        else
        {
            prevIndex = index - 1;
        }

        return prevIndex;
    }

    public static T PrevItem<T>(this List<T> list, int index)
    {
        return list[list.PrevIndex(index)];
    }


#if UNITY_EDITOR
    /* Example:

	using UnityEditor;
	public static class SetDefineUniRX {
		[InitializeOnLoadMethod]
		public static void Init() {
			ExtensionsMisc.SetDefineSymbolOnBuildTarget( EditorUserBuildSettings.selectedBuildTargetGroup, "UNIRX" );
		}
	}

	*/
    public static void SetDefineSymbolOnBuildTarget(UnityEditor.BuildTargetGroup targetGroup, string defineStr)
    {
        string currData = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        if (!currData.Contains(defineStr))
        {
            if (string.IsNullOrEmpty(currData))
            {
                UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defineStr);
            }
            else
            {
                if (!currData[currData.Length - 1].Equals(';'))
                {
                    currData += ';';
                }

                currData += defineStr;
                Debug.Log("SetDefineSymbolOnBuildTarget: " + defineStr);
                UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, currData);
            }
        }
    }
#endif

    // Cannot yet have extensions methods change the value sent in say by ref
    public static ComputeShader Find(this ComputeShader compute, string computeSearchName)
    {
#if UNITY_EDITOR
        var guids = UnityEditor.AssetDatabase.FindAssets(computeSearchName, null);
        if (guids != null && guids.Length >= 1)
        {
            foreach (var guid in guids)
            {
                //Debug.Log( "guid: " + guid );
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                if (path.EndsWith(".compute"))
                {
                    Debug.Log("path: " + path);
                    compute = UnityEditor.AssetDatabase.LoadAssetAtPath<ComputeShader>(path);
                    break;
                }
            }
        }

        if (compute == null) Debug.LogError("ComputeShader Find Failed:" + computeSearchName);
#endif
        return compute;
    }

    public static string[] Split(this string stringIn, string splitter, StringSplitOptions options = StringSplitOptions.None)
    {
        return stringIn.Split(new string[] {splitter}, options);
    }

    public static string SplitAndTakeLast(this string stringIn, string splitter, StringSplitOptions options = StringSplitOptions.None)
    {
        return stringIn.Split(new string[] {splitter}, options).Last();
    }

    public static string SplitAndTakeFirst(this string stringIn, string splitter, StringSplitOptions options = StringSplitOptions.None)
    {
        return stringIn.Split(new string[] {splitter}, options).First();
    }

    public static string ArrayToString(this Array someArray, string delim = " , ", string prefix = "")
    {
        //string strout = "";
        //foreach( var item in someArray ) {
        //string delim = " , ";
        for (int i = 0; i < someArray.Length; i++)
        {
            var item = someArray.GetValue(i);
            if (i == 0)
            {
                prefix += item.ToString(); // SHould use StringBuilder
            }
            else
            {
                prefix += delim + item.ToString();
            }
        }

        return prefix;
    }


    /*
	public static void AddItemIfNotPresent<T>( this List<T> list, Behaviour obj, bool enableIfFound = true ) where T : ItemWrap<Behaviour>, new() {
		bool found = false;
		if( list == null ) list = new List<T>();
		foreach( var o in list ) {
			if( Equals( o.Item, obj ) ) {
				found = true;
				if( enableIfFound)o.enabled = true;
			}
		}
		if( !found ) {
			list.Add( new T { Item = obj } );
			//list.Add( new T(obj) );
		}
	}*/

    public static void AddIfNotPresent<T>(this List<T> list, T obj, bool debug = false)
    {
        if (!list.Contains(obj))
        {
            if (debug) Debug.Log("AddIfNotPresent: " + obj);
            list.Add(obj);
        }
        else
        {
            if (debug) Debug.Log("AddIfNotPresent Already Contains: " + obj);
        }
    }

    public static double ToDouble<T>(this T value) where T : struct
    {
        return Convert.ToDouble(value);
    }


    /// <summary>
    /// Use RemoveAll(o => o == null); instead?
    /// </summary>
    public static List<T> RemoveEmpty<T>(this List<T> list)
    {
        list.RemoveAll(o => o == null);
        return list;
        /*
        if( list == null || list.Count == 0 ) return list; // Could warn if null or create an empty list
        //Debug.LogWarning("RemoveEmpty: " + list.Count);
        for ( int i = list.Count - 1; i >= 0; i-- ) {
            //Debug.LogWarning("RemoveEmptyH:" + i+" : "+(list[i] == null) );
            if( list[i] == null  ) list.RemoveAt( i );
        }
        return list;*/
    }

    public static List<T> RemoveEmptyBehaviour<T>(this List<T> list) where T : Behaviour
    {
        if (list == null || list.Count == 0) return list; // Could warn if null or create an empty list
        //Debug.LogWarning("RemoveEmpty: " + list.Count);
        for (int i = list.Count - 1; i >= 0; i--)
        {
            //Debug.LogWarning("RemoveEmptyH:" + i+" : "+(list[i] == null) );
            if (list[i] == null) list.RemoveAt(i);
        }

        return list;
    }

    [Obsolete("Use RemoveEmpty()")]
    public static List<T> RemoveNull<T>(this List<T> listIn)
    {
        if (listIn == null) return listIn; // Could warn if null or create an empty list
        for (int i = listIn.Count - 1; i >= 0; i--)
        {
            var item = listIn[i];
            if (item == null) listIn.RemoveAt(i);
        }

        return listIn;
    }

    //[NonSerialized]
    //public static Dictionary<String, BufferManagerItem> bufferCounter = new Dictionary<String, BufferManagerItem>();

    public static int someInt;

    /*

	private static BufferManagerItem NewComputerBuffer( this ComputeBuffer buffer, string nameIn, int itemCount, int stride, out ComputeBuffer output, GameObject go ) {
		if ( buffer != null ) buffer.Release();
		if ( buffer != null ) buffer.Dispose();
		buffer = new ComputeBuffer( itemCount, stride );


#if UNITY_EDITOR
		if ( bufferCounter == null ) bufferCounter = new Dictionary<String, BufferManagerItem>();
		if ( !bufferCounter.ContainsKey( nameIn ) ) bufferCounter.Add( nameIn, new BufferManagerItem( buffer, 0, itemCount, stride ) );
		Debug.Log("meh:"+ bufferCounter.ContainsKey( nameIn ) +":"+ nameIn );
		var o = bufferCounter[nameIn];
		if ( go != null ) if ( !o.gameObjects.Contains( go ) ) o.gameObjects.Add( go );
		var counter = o.counter += 1;
		//if( counter > 1 )
		if ( go != null ) {
			Debug.Log( "New Buffer '" + nameIn + "' count: " + itemCount + " go: " + go + " counter: " + counter, go );
		} else {
			Debug.Log( "New Buffer '" + nameIn + "' count: " + itemCount + " counter: " + counter, go );
		}
		bufferManagerEvent.Invoke();
#endif

		output = buffer;
		return o;
	}

	public static void NewComputerBuffer( this ComputeBuffer buffer, string nameIn, int itemCount, System.Type type, out ComputeBuffer output, GameObject go ) {
		var o = NewComputerBuffer( buffer, nameIn, itemCount, type.StructLength(), out buffer, go );
		o.type = type;
		o.typeName = type.Name;
		output = buffer;
	}*/

    public static int StructLength(this System.Type value)
    {
        // Store Marshal Size in a Dict? Table?
        var length = Marshal.SizeOf(value);
#if UNITY_EDITOR

        if (length % Marshal.SizeOf(typeof(Vector4)) > 0)
        {
            if (value.Name == "Int32") return length; // Skip for Int32 as likely done on purpose

            if (!Application.isPlaying) Debug.LogWarning(value.Name + " Length is : (" + length + ") Structs should be divisible by float4 (" + Marshal.SizeOf(typeof(Vector4)) + ")");
        }
        else
        {
            //Debug.Log( "StructLength: " + value.Name + ":" + length );
        }
#endif
        return length;
    }
    /*
	public static int StructLength( this System.Type value, string nameIn ) {
		var length = Marshal.SizeOf( value );
#if UNITY_EDITOR
		if( length % Marshal.SizeOf( typeof( float ) ) > 0 ) {
			Debug.LogWarning( nameIn + " Length is : " + length + " Structs should be divisible by float4 (" + Marshal.SizeOf( typeof( float ) ) + ")" );
		} else {
			Debug.Log("Struct:ength: " + nameIn);
		}
#endif
		return length;
	}*/

    // http://www.alanzucconi.com/2015/08/05/extension-methods-in-c/
    public static bool Freeze(this Rigidbody2D rigidbody2D)
    {
        if (rigidbody2D == null) return false;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0;
        rigidbody2D.isKinematic = true;
        return true;
    }

    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    public static T AssetFindOrCreate<T>(this T so, string path = "Assets/Resources/", bool mustBeInPath = false, bool createFile = false) where T : UnityEngine.ScriptableObject
    {
        //TODO handle being sent specific filename, and try load from resources at runtime

        T existingAsset = null;

        string foundGUID = null;

        var type = typeof(T);

        Debug.Log("AssetFindOrCreate:" + type);

#if UNITY_EDITOR

        var pathsFound = UnityEditor.AssetDatabase.FindAssets("t:" + type.ToString()).ToList();

        if (pathsFound.Count != 0)
        {
            if (!mustBeInPath) foundGUID = pathsFound.First();

            foreach (var pathFound in pathsFound)
            {
                if (pathFound.Contains(path))
                {
                    foundGUID = pathFound;
                }
            }

            Debug.Log("AssetFindOrCreate: foundGUID:" + foundGUID);
            var foundPath = UnityEditor.AssetDatabase.GUIDToAssetPath(foundGUID);
            Debug.Log("AssetFindOrCreate: foundPath:" + foundPath);
            if (!string.IsNullOrEmpty(foundGUID)) existingAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(foundPath);
        }
        else
        {
            // None found so create
        }


        if (existingAsset == null)
        {
            T newso = ScriptableObject.CreateInstance(type) as T;
            if (createFile) UnityEditor.AssetDatabase.CreateAsset(newso, path);
            existingAsset = newso;
        }
        else
        {
            //UnityEditor.EditorUtility.CopySerialized( asset, existingAsset );
        }

        so = existingAsset;
#else
		Debug.LogError( "AssetFindOrCreate can only be used in Editor: " + type+":"+path );
#endif


        return existingAsset;
    }

#if UNITY_EDITOR

    // http://answers.unity3d.com/questions/24929/assetdatabase-replacing-an-asset-but-leaving-refer.html
    public static T CreateOrReplaceAssetE<T>(this T asset, string path) where T : UnityEngine.Object
    {
        return CreateOrReplaceAsset(asset, path);
    }

    public static T CreateOrReplaceAsset<T>(T asset, string path) where T : UnityEngine.Object
    {
        T existingAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);

        if (existingAsset == null)
        {
            UnityEditor.AssetDatabase.CreateAsset(asset, path);
            existingAsset = asset;
        }
        else
        {
            UnityEditor.EditorUtility.CopySerialized(asset, existingAsset);
        }

        return existingAsset;
    }


    public static void ListDebugContents(this IEnumerable<GameObject> gos)
    {
        Debug.Log("---- ListDebugContents: " + gos.Count() + " ----");
        int i = 0;
        foreach (var go in gos)
        {
            if (go == null)
            {
                Debug.Log("\tDL: " + i + " : " + "-- null entry --");
                continue;
            }

            Debug.Log("\tDL: " + i + " : " + go.name, go);
            i++;
        }
    }

    public static void ListContentsDebug(this UnityEditor.SerializedObject so)
    {
        ListContentsDebug(so.GetIterator());
        /*var it = so.GetIterator();
		int i = 0;
		while ( it != null ) {
			it.Next( true );
			i++;
			Debug.Log( "it:" + i + ":" + it.displayName + " : " + it.name );
			if ( i > 200 ) return;
		}*/
    }

    public static void ListContentsDebug(this UnityEditor.SerializedProperty sp)
    {
        Debug.Log("---- ListContentsDebug ----");
        var it = sp;
        it.Reset();
        int i = 0;
        while (it != null)
        {
            if (it == null) break;
            var success = it.Next(true);
            if (it == null || !success) break;
            i++;
            Debug.Log("\tit:" + i + ":" + it.displayName + " : " + it.name + " : " + it.propertyPath);
            if (i > 200) break;
        }

        it.Reset();
    }

    public static UnityEditor.SerializedProperty FindPropertyByName(this UnityEditor.SerializedProperty sp, string needle)
    {
        var it = sp;
        it.Reset();
        Debug.Log("---- FindPropertyByName: '" + needle + "'----");
        int i = 0;
        while (it != null)
        {
            // if ( i >= it.arraySize ) break;
            if (it == null) break;
            var success = it.Next(true);

            if (!success) break;
            Debug.Log("\tfp:" + i + ":" + it.displayName + " : " + it.name);
            if (it.name == needle)
            {
                it.Reset();
                return it;
            }

            i++;
            //
            if (i > 200) break;
        }

        it.Reset();
        return null;
    }

#endif

    public static bool ComputeIsEmpty(this ComputeBuffer buffer, int desiredLength = -1, int desireStride = -1)
    {
        try
        {
            if (buffer == null || buffer.count == 0) return true; // || buffer.count == 0 // doesnt work get null exception
            if (desiredLength > 0 && buffer.count != desiredLength) return true;
            if (desireStride > 0 && buffer.stride != desireStride) return true;
        }
        catch
        {
            // Having to rely on exception as only way to tell if buffer is disposed?
            return true;
        }

        return false;
    }

    public static bool ComputeIsntEmpty(this ComputeBuffer buffer)
    {
        if (buffer == null) return false;
        return true;
    }

    public static void SetActiveSafe(this GameObject obj, bool value)
    {
        if (obj.activeSelf != value) obj.SetActive(value);
    }

    public static void AddIfNotNull<T>(this List<T> someList, T someT)
    {
        if (someT != null) someList.Add(someT);
    }

    public static void CopyTransform(this Transform t, Transform target)
    {
        t.position = target.position;
        t.rotation = target.rotation;
        t.localScale = target.localScale;
    }

    public static void CopyTransform(this GameObject go, GameObject target)
    {
        go.transform.position = target.transform.position;
        go.transform.rotation = target.transform.rotation;
        go.transform.localScale = target.transform.localScale;
    }

    public static void ResetTransform(this GameObject go)
    {
        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
    }

    public static void ResetTransform(this Transform t)
    {
        t.position = Vector3.zero;
        t.rotation = Quaternion.identity;
        t.localScale = Vector3.one;
    }

    public static void ResetTransformLocal(this GameObject go)
    {
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
    }

    public static void ResetTransformLocal(this Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
    }

    //

    public static Quaternion ToQuaternion(this Vector4 v)
    {
        return new Quaternion(v.x, v.y, v.z, v.w);
    }

    public static Quaternion ToNormalizedQuaternion(this Vector4 v)
    {
        v = Vector4.Normalize(v);
        return new Quaternion(v.x, v.y, v.z, v.w);
    }


    public static Vector4 ToVector4(this Quaternion q)
    {
        return new Vector4(q.x, q.y, q.z, q.w);
    }


    //

    public static List<Transform> GetChildren(this Transform t)
    {
        var list = new List<Transform>();
        for (int i = 0; i < t.childCount; i++)
        {
            list.Add(t.GetChild(i));
        }

        return list;
    }

    public static List<GameObject> GetChildren(this GameObject go)
    {
        var list = new List<GameObject>();
        for (int i = 0; i < go.transform.childCount; i++)
        {
            list.Add(go.transform.GetChild(i).gameObject);
        }

        return list;
    }

    /// <summary>
    /// Use for SetLength, Get, given val/instance count workout how many instances round to the kThread, so given 67 val and kThread 64 it will return 128
    /// </summary>
    public static int ThreadCountLength(this int val, int kThreadCount = 64)
    {
        int newcount = Mathf.Max((val + kThreadCount - 1) / kThreadCount, 1); // Round up int
        return newcount * kThreadCount;
    }

    /// <summary>
    /// Use for Dispatch: Given val/instance count workout how many threads need to be dispatched given a kThreadCount size eg given 67 val and kThread 64 it will return 128/64 == 2 threads
    /// </summary>
    public static int ThreadCountBatch(this int val, int kThreadCount = 64)
    {
        int newcount = Mathf.Max((val + kThreadCount - 1) / kThreadCount, 1); // Round up int
        return newcount;
    }

    public static int RoundUpInt(this int val, int roundVal = 64)
    {
        int newcount = Mathf.Max((val + roundVal - 1) / roundVal, 1); // Round up int
        return newcount * roundVal;
    }


    /// <summary>
    /// Superlame hacky HASH for curves, use sparingly, use sparingly for editor use only ( generates GC )
    /// </summary>
    public static int HashCurve(this AnimationCurve curve)
    {
        //string hasstr = "";
        float hackyhash = 0;
        int i = 0;
        foreach (var keyframe in curve.keys)
        {
            i++;
            //hasstr += keyframe.value + ":" + keyframe.time + ":" + keyframe.inTangent + ":" + keyframe.outTangent;
            hackyhash += (keyframe.value + (8 * keyframe.time) + (16 * keyframe.inTangent) + (32 * keyframe.outTangent)) * i * 1000;
        }

        //return hackyhash.GetHashCode();
        return (int) hackyhash;
    }

    /// <summary>
    /// Superlame hacky HASH for gradients, use sparingly for editor use only ( generates GC )
    /// </summary>
    public static int HashGradient(this Gradient gradient)
    {
        float hackyhash = 0;
        int i = 0;
        foreach (var keyframe in gradient.colorKeys)
        {
            i++;
            hackyhash += (keyframe.color.r + (8 * keyframe.time)) + (keyframe.color.g + (16 * keyframe.time)) + (keyframe.color.b + (32 * keyframe.time)) * i * 1000;
        }

        foreach (var keyframe in gradient.alphaKeys)
        {
            i++;
            hackyhash += (keyframe.alpha * 64) * i * 1000;
        }

        return (int) hackyhash;
    }

    public static float LargestVal(this AnimationCurve curve)
    {
        float val = 0;
        foreach (var keyframe in curve.keys)
        {
            if (keyframe.value > val) val = keyframe.value;
        }

        return val;
    }

    public static float SmallestVal(this AnimationCurve curve)
    {
        float val = 0;
        foreach (var keyframe in curve.keys)
        {
            if (keyframe.value < val) val = keyframe.value;
        }

        return val;
    }

    public static float LargestTime(this AnimationCurve curve)
    {
        float val = 0;
        foreach (var keyframe in curve.keys)
        {
            if (keyframe.time > val) val = keyframe.time;
        }

        return val;
    }

    public static float SmallestTime(this AnimationCurve curve)
    {
        float val = 0;
        foreach (var keyframe in curve.keys)
        {
            if (keyframe.time < val) val = keyframe.time;
        }

        return val;
    }

    public static AnimationCurve NormalizeNOTIMPLIMENTED(this AnimationCurve curve)
    {
        return curve;
    }

    //
    /*
	public static Vector3[] Add( this Vector3[] v1, Vector3[] v2, bool exactSize = true ) {
		if ( exactSize && v1.Length != v2.Length ) Debug.LogWarning( "Vector3 Add arrays not the same length" );
		for ( int i = 0; i < v1.Length; i++ ) {
			if ( i > v2.Length ) {
				break;
			}
			v1[i] += v2[i];
		}
		return v1;
	}*/

    /*
	// public static void AddIfNotNull<T>( this List<T> someList, T someT ) {
	public static T[] Add<T>( this T[] v1, T[] v2, bool exactSize = true ) where T : Ve {
		if( exactSize && v1.Length != v2.Length ) Debug.LogWarning( "Vector4 Add arrays not the same length" );
		for( int i = 0; i < v1.Length; i++ ) {
			if( i > v2.Length ) {
				break;
			}
			v1[i] += v2[i];
		}
		return v1;
	}*/
    /*
	public static T[] Add<T>( this T[] a, T[] b ) {
		return a;
	}*/

    public static void DebugLog<T>(this List<T> a, string header = "DebugLog IEnumerable", GameObject go = null, int max = -1, int skip = -1)
    {
        Debug.Log("--- " + header + " ---", go);
        int jj = 0;
        for (int j = 0; j < a.Count(); j++)
        {
            if (max > 0 && jj > max) break;
            if (skip > 0)
            {
                Debug.Log("\t\t(" + j + ") (" + jj + ") " + a[j], go);
                j += skip;
            }
            else
            {
                Debug.Log("\t\t(" + j + ") " + a[j], go);
            }

            jj++;
        }
    }

    public static void DebugLog<T>(this T[] a, string header = "DebugLog IEnumerable", GameObject go = null, int max = -1, int skip = -1)
    {
        Debug.Log("--- " + header + " ---", go);
        int jj = 0;
        for (int j = 0; j < a.Length; j++)
        {
            if (max > 0 && jj > max) break;
            if (skip > 0)
            {
                Debug.Log("\t\t(" + j + ") (" + jj + ") " + a[j], go);
                j += skip;
            }
            else
            {
                Debug.Log("\t\t(" + j + ") " + a[j], go);
            }

            jj++;
        }
    }

    public static void DebugLog<T>(this IEnumerable<T> a, string header = "DebugLog IEnumerable", GameObject go = null, int max = -1)
    {
        Debug.Log("--- " + header + " ---", go);
        int i = 0;
        foreach (var item in a)
        {
            i++;
            if (max > 0 && i > max) break;
            Debug.Log("\t\t(" + i + ")" + item, go);
        }
    }

    public static string DebugString<T>(this IEnumerable<T> a, string header = "DebugString IEnumerable", GameObject go = null, int max = -1)
    {
        Debug.Log("--- " + header + " ---", go);
        int i = 0;
        var str = new StringBuilder();
        foreach (var item in a)
        {
            i++;
            if (max > 0 && i > max) break;
            //Debug.Log( "\t\t(" + i + ")" + item, go );
            str.Append("(" + i + ")" + item + ", ");
        }

        str.Remove(str.Length - 2, 2);
        return str.ToString();
    }

    public static string StringCollapse<T>(this IEnumerable<T> a, string seperator = ", ", bool prefixNumber = false, int max = -1)
    {
        int i = 0;
        var str = new StringBuilder();

        foreach (var item in a)
        {
            i++;
            if (max > 0 && i > max) break;
            //Debug.Log( "\t\t(" + i + ")" + item, go );
            if (prefixNumber)
            {
                str.Append("(" + i + ")" + item + seperator);
            }
            else
            {
                str.Append(item + seperator);
            }
        }

        str.Remove(str.Length - seperator.Length, seperator.Length); // Remove last seperator
        return str.ToString();
    }

    public static string StringCollapseGameObject(this IEnumerable<GameObject> a, string seperator = ", ", bool prefixNumber = false, int max = -1)
    {
        int i = 0;
        var str = new StringBuilder();

        foreach (var item in a)
        {
            i++;
            if (max > 0 && i > max) break;
            //Debug.Log( "\t\t(" + i + ")" + item, go );
            if (prefixNumber)
            {
                str.Append("(" + i + ")" + item.name + seperator);
            }
            else
            {
                str.Append(item.name + seperator);
            }
        }

        str.Remove(str.Length - seperator.Length, seperator.Length); // Remove last seperator
        return str.ToString();
    }

    // TODO does newer C Sharp allow use of 'ref' in extensions methods to set the orignal value?
    public static T[] AddArraysToOut<T>(this T[] a, T[] b, out T[] output, bool lengthsMustMatch = true)
    {
        if (lengthsMustMatch && a.Length != b.Length)
        {
            Debug.LogWarning("Lengths dont match");
        }

        int i = 0;
        var addFunc = AddFunc<T>();
        while (i < a.Length && i < b.Length)
        {
            a[i] = a[i].AddAny(b[i], addFunc);
            i++;
        }

        output = a;
        return a;
    }

    public static List<T> AddListsToRef<T>(this List<T> a, ref List<T> b, bool lengthsMustMatch = true)
    {
        if (lengthsMustMatch && a.Count != b.Count)
        {
            Debug.LogWarning("Lengths dont match");
        }

        int i = 0;
        var addFunc = AddFunc<T>();
        while (i < a.Count && i < b.Count)
        {
            a[i] = a[i].AddAny(b[i], addFunc);
            i++;
        }

        b = a;
        return a;
    }

    public static T[] AddArraysToRef<T>(this T[] a, ref T[] b, bool lengthsMustMatch = true)
    {
        if (lengthsMustMatch && a.Length != b.Length)
        {
            Debug.LogWarning("Lengths dont match");
        }

        int i = 0;
        var addFunc = AddFunc<T>();
        while (i < a.Length && i < b.Length)
        {
            //a[i] = a[i].Add( b[i] );
            a[i].AddToRef(ref b[i], addFunc);
            i++;
        }

        //b = a; // TODO change to Add to Ref B
        return a;
    }

    private static Func<Vector3, Vector3, Vector3> addVec3;

    public static Vector3[] AddArrayVec3ToMe(this Vector3[] a, Vector3[] b, bool lengthsMustMatch = true)
    {
        if (addVec3 == null) addVec3 = AddFunc<Vector3>(funcWarn: false);
        a.AddArrayToMe(b, addFuncIn: addVec3, funcWarn: false);
        return a;
    }

    private static Func<Vector4, Vector4, Vector4> addVec4;

    public static Vector4[] AddArrayVec3ToMe(this Vector4[] a, Vector4[] b, bool lengthsMustMatch = true)
    {
        if (addVec4 == null) addVec4 = AddFunc<Vector4>(funcWarn: false);
        a.AddArrayToMe(b, addFuncIn: addVec4, funcWarn: false);
        return a;
    }

    public static T[] AddArrayToMe<T>(this T[] a, T[] b, bool lengthsMustMatch = true, Func<T, T, T> addFuncIn = null, bool funcWarn = true)
    {
        if (lengthsMustMatch && a.Length != b.Length)
        {
            Debug.LogWarning("Lengths dont match");
        }

        int i = 0;
        Func<T, T, T> addFunc = addFuncIn;
        if (addFunc == null) addFunc = AddFunc<T>(funcWarn);
        while (i < a.Length && i < b.Length)
        {
            a[i].AddToOut(b[i], out a[i], addFunc);
            //a[i].AddToMe( b[i] ); // Wont work as data isnt ref type/boxed

            //a[i] = a[i].Add( b[i] );
            i++;
        }

        //b = a; // TODO change to Add to Ref B
        return a;
    }
    /*
	public static Vector3[] AddArray<T>( this Vector3[] a, Vector3[] b, out Vector3[] output ) {
		if ( a.Length != b.Length ) {
			Debug.LogWarning( "Lengths dont match" );
		}
		int i = 0;
		while ( i < a.Length && i < b.Length ) {
			a[i] += b[i];
			i++;
		}
		output = a;
		return a;

	}
	public static Vector4[] AddArray<T>( this Vector4[] a, Vector4[] b, out Vector4[] output ) {
		if ( a.Length != b.Length ) {
			Debug.LogWarning( "Lengths dont match" );
		}
		int i = 0;
		while ( i < a.Length && i < b.Length ) {
			a[i] += b[i];
			i++;
		}
		output = a;
		return a;
	}*/

    /* // TODO cannot modify contents ofIEnumerable while iterating?
public static IEnumerable<T> AddEnumerableToRef<T>( this IEnumerable<T> a, ref IEnumerable<T> b ) {
	if ( a.Count() != b.Count() ) {
		Debug.LogWarning( "Lengths dont match" );
	}
	int i = 0;
	var ae = a.GetEnumerator();
	var be = b.GetEnumerator();
	//while ( i < a.Count() && i < b.Count() ) {
	while( ae.MoveNext() && be.MoveNext() ) {
		// ae.MoveNext();
		//be.MoveNext();

		///a.ElementAt( i ) = a.ElementAt( i );
		//ae.Current = ae.Current.Add<T>( be.Current );
		i++;
	}
	ae.Dispose();
	be.Dispose();
	b = a;
	return a;
}*/

    /// <summary>
    /// Be mindful of boxing!
    /// Do not use this with Lists, Arrays, use AddArraysToRef instead
    /// </summary>
    public static T AddAny<T>(this T a, T b, Func<T, T, T> addFunc)
    {
        /*
		//TODO: re-use delegate!
		// declare the parameters
		ParameterExpression paramA = Expression.Parameter( typeof(T), "a" ), paramB = Expression.Parameter( typeof(T), "b" );
		// add the parameters together
		BinaryExpression body = Expression.Add( paramA, paramB );
		// compile it
		Func<T, T, T> add = Expression.Lambda<Func<T, T, T>>( body, paramA, paramB ).Compile();
		// call it*/
        return addFunc(a, b);
    }

    /// <summary>
    /// Be mindful of boxing!
    /// Adds a to b and sets b to the total and returns the total
    /// Do not use this with Lists, Arrays, use AddArraysToRef instead
    /// </summary>
    public static T AddToRef<T>(this T a, ref T b, Func<T, T, T> addFunc)
    {
        //TODO: re-use delegate!
        // declare the parameters
        /*Func<T, T, T> add = addFunc;
		if ( addFunc == null ) {
			ParameterExpression paramA = Expression.Parameter( typeof(T), "a" ), paramB = Expression.Parameter( typeof(T), "b" );
			// add the parameters together
			BinaryExpression body = Expression.Add( paramA, paramB );
			// compile it
			add = Expression.Lambda<Func<T, T, T>>( body, paramA, paramB ).Compile();
		}*/
        // call it
        return b = addFunc(a, b);
    }

    public static Func<T, T, T> AddFunc<T>(bool funcWarn = true)
    {
        if (funcWarn) Debug.LogWarning("AddFuncIn is expensive, be warned");
        //TODO: re-use delegate!
        // declare the parameters

        ParameterExpression paramA = Expression.Parameter(typeof(T), "a"), paramB = Expression.Parameter(typeof(T), "b");
        // add the parameters together
        BinaryExpression body = Expression.Add(paramA, paramB);
        // compile it
        Func<T, T, T> add = Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();

        // call it
        return add;
    }

    /*
	/// <summary>
	/// Be mindful of boxing!
	/// Adds a to b setting a to total
	/// Do not use this with Lists, Arrays, use AddArraysToRef instead
	/// </summary>
	public static T AddToMe<T>( this T a, T b ) {

		//TODO: re-use delegate!
		// declare the parameters
		ParameterExpression paramA = Expression.Parameter( typeof(T), "a" ), paramB = Expression.Parameter( typeof(T), "b" );
		// add the parameters together
		BinaryExpression body = Expression.Add( paramA, paramB );
		// compile it
		Func<T, T, T> add = Expression.Lambda<Func<T, T, T>>( body, paramA, paramB ).Compile();
		// call it
		a = add( a, b );
		return a;
	}*/

    /// <summary>
    /// Be mindful of boxing!
    /// Adds a to b outputting total to 'output' and returns a
    /// Do not use this with Lists, Arrays, use AddArraysToRef instead
    /// Very slow if you do not re use the delegate somehow
    /// </summary>
    public static T AddToOut<T>(this T a, T b, out T output, Func<T, T, T> addFunc)
    {
        /*
		//TODO: re-use delegate!
		// declare the parameters
		ParameterExpression paramA = Expression.Parameter( typeof(T), "a" ), paramB = Expression.Parameter( typeof(T), "b" );
		// add the parameters together
		BinaryExpression body = Expression.Add( paramA, paramB );
		// compile it
		Func<T, T, T> add = Expression.Lambda<Func<T, T, T>>( body, paramA, paramB ).Compile();
		// call it*/
        output = addFunc(a, b);
        return a;
    }

    /// <summary>
    /// Take in A and a Ref B, set B's contents to A, returns A
    /// </summary>
    public static T SetRef<T>(this T a, ref T b)
    {
        b = a;
        return a;
    }

    /// <summary>
    /// Take in A and a Ref B, set B's contents to A, returns B
    /// </summary>
    public static T SetAndReturnRef<T>(this T a, ref T b)
    {
        b = a;
        return b;
    }

    // Warning, below SetXYZ on structs/vectors and such will not affect the orignial value as it is passed in by value, not ref,

    public static Vector3 SetXYZ(this Vector3 v, float x, float y, float z)
    {
        //v.x = x;
        //v.y = y;
        //v.z = z;
        v.Set(x, y, z);
        return v;
    }

    public static Vector3 SetXY(this Vector3 v, float x, float y)
    {
        //v.x = x;
        //v.y = y;
        v.Set(x, y, v.z);
        return v;
    }

    public static Vector3 SetXZ(this Vector3 v, float x, float z)
    {
        //v.x = x;
        //v.z = z;
        v.Set(x, v.y, z);
        return v;
    }

    public static Vector3 SetYZ(this Vector3 v, float y, float z)
    {
        //v.x = y;
        //v.z = z;
        v.Set(v.x, y, z);
        return v;
    }

    public static Vector2 SetX(this Vector2 vec, float x)
    {
        vec.x = x;
        return vec;
    }

    public static Vector2 SetY(this Vector2 vec, float y)
    {
        vec.y = y;
        return vec;
    }

    //

    public static Vector3 SetX(this Vector3 vec, float x)
    {
        vec.x = x;
        return vec;
    }

    public static Vector3 SetY(this Vector3 vec, float y)
    {
        vec.y = y;
        return vec;
    }

    public static Vector3 SetZ(this Vector3 vec, float z)
    {
        vec.z = z;
        return vec;
    }

    //

    public static Vector4 SetX(this Vector4 vec, float x)
    {
        vec.x = x;
        return vec;
    }

    public static Vector4 SetY(this Vector4 vec, float y)
    {
        vec.y = y;
        return vec;
    }

    public static Vector4 SetZ(this Vector4 vec, float z)
    {
        vec.z = z;
        return vec;
    }

    public static Vector4 SetW(this Vector4 vec, float w)
    {
        vec.w = w;
        return vec;
    }

    public static Vector4 SetXYZ(this Vector4 v, float x, float y, float z)
    {
        //v.x = x;
        //v.y = y;
        //v.z = z;
        v.Set(x, y, z, v.w);
        return v;
    }

    public static Vector4 SetXY(this Vector4 v, float x, float y)
    {
        //v.x = x;
        //v.y = y;
        v.Set(x, y, v.z, v.w);
        return v;
    }

    public static Vector4 SetXYZ(this Vector4 v, Vector3 vec3)
    {
        v.Set(vec3.x, vec3.y, vec3.z, v.w);
        return v;
    }

    //

    public static Transform SetXYZ(this Transform t, float x, float y, float z)
    {
        t.position = t.position.SetXYZ(x, y, z);
        return t;
    }

    public static Transform SetXYZ(this Transform t, Vector3 vec3)
    {
        t.position = t.position.SetXYZ(vec3.x, vec3.y, vec3.z);
        return t;
    }

    public static Transform SetX(this Transform t, float x)
    {
        t.position = t.position.SetX(x);
        return t;
    }

    public static Transform SetY(this Transform t, float y)
    {
        t.position = t.position.SetY(y);
        return t;
    }

    public static Transform SetZ(this Transform t, float z)
    {
        t.position = t.position.SetZ(z);
        return t;
    }

    public static Transform SetLocalScaleX(this Transform t, float x)
    {
        t.localScale = t.localScale.SetX(x);
        return t;
    }

    public static Transform SetLocalScaleY(this Transform t, float y)
    {
        t.localScale = t.localScale.SetY(y);
        return t;
    }

    public static Transform SetLocalScaleZ(this Transform t, float z)
    {
        t.localScale = t.localScale.SetZ(z);
        return t;
    }

    public static GameObject SetXYZ(this GameObject t, float x, float y, float z)
    {
        t.transform.SetXYZ(x, y, z);
        return t;
    }

    public static GameObject SetX(this GameObject t, float x)
    {
        t.transform.SetX(x);
        return t;
    }

    public static GameObject SetY(this GameObject t, float y)
    {
        t.transform.SetY(y);
        return t;
    }

    public static GameObject SetZ(this GameObject t, float z)
    {
        t.transform.SetZ(z);
        return t;
    }

    public static GameObject SetLocalX(this GameObject t, float x)
    {
        t.transform.localPosition = t.transform.localPosition.SetX(x);
        return t;
    }

    public static GameObject SetLocalY(this GameObject t, float y)
    {
        t.transform.localPosition = t.transform.localPosition.SetY(y);
        return t;
    }

    public static GameObject SetLocalZ(this GameObject t, float z)
    {
        t.transform.localPosition = t.transform.localPosition.SetZ(z);
        return t;
    }

    public static Transform SetLocalX(this Transform t, float x)
    {
        t.transform.localPosition = t.localPosition.SetX(x);
        return t;
    }

    public static Transform SetLocalY(this Transform t, float y)
    {
        t.transform.localPosition = t.localPosition.SetY(y);
        return t;
    }

    public static Transform SetLocalZ(this Transform t, float z)
    {
        t.transform.localPosition = t.localPosition.SetZ(z);
        return t;
    }
}
