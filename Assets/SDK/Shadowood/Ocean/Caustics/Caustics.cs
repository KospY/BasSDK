using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;

namespace Shadowood
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    [ExecuteInEditMode]
    public class Caustics : MonoBehaviour
    {
        internal const string FOLDER_PATH = "Assets/SDK/Shadowood";

        public bool debugCaustics;

        [GUIColor("@PathValid() ? Color.white : Color.red")] [Space]
        public string texturesPath = FOLDER_PATH + @"/Ocean/Caustics/C.Sequence/";

        public static readonly int CausticMatrix = Shader.PropertyToID("_CausticMatrix");
        public static readonly int CausticsSettings = Shader.PropertyToID("_CausticsSettings");
        public static readonly int CausticsPropName = Shader.PropertyToID("_Caustics");

        public static readonly int CausticsColorPropName = Shader.PropertyToID("_CausticsColor");
        //public static readonly int CausticsDirPropName = Shader.PropertyToID("_CausticsDir");
        //public static readonly int CausticsScalePropName = Shader.PropertyToID("_CausticsScale");

        //public const string causticPropName = "_Caustics";
        //public const string causticColorPropName = "_CausticsColor";
        //public const string causticDirPropName = "_CausticsDir";
        //public const string causticScalePropName = "_CausticsScale";

#if UNITY_EDITOR
        //TODO make singletonn, warn if multiple found?

        public bool PathValid()
        {
            return System.IO.Directory.Exists(texturesPath);
        }

        [ContextMenu("FindTextures")]
        public void FindTextures()
        {
            if (!System.IO.Directory.Exists(texturesPath))
            {
                Debug.LogError("Path doesnt exist: " + texturesPath);
                return;
            }

            causticTextures = GetAssetsAtPath<Texture>(texturesPath);
        }

        public static T[] GetAssetsAtPath<T>(string path) where T : UnityEngine.Object
        {
            var assets = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] {path});
            List<T> foundAssets = new List<T>();
            foreach (var guid in assets)
            {
                foundAssets.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid)));
            }

            return foundAssets.ToArray();
        }

        static Caustics()
        {
            UnityEditor.EditorApplication.update += DisableLater;
        }

        private static void DisableLater()
        {
            UnityEditor.EditorApplication.update -= DisableLater;
            SetDefaults();
        }


        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            DisableLater();
            foreach (var o in FindObjectsOfType<Caustics>())
            {
                if (o == null) continue;
                if (o.isActiveAndEnabled) o.PropertyUpdate();
            }
        }

        private void PropertyUpdateLater()
        {
            UnityEditor.EditorApplication.update -= PropertyUpdateLater;
            PropertyUpdate();
        }

#endif

        /// <summary>
        /// Runs after OnEnable on script reload, turns caustics off
        /// </summary>
        public static void SetDefaults()
        {
            Shader.SetGlobalColor(CausticsColorPropName, Color.black);
        }

        [GUIColor("@causticTextures.Length > 0 ? Color.white : Color.red")] [InlineButton("FindTextures")]
        public Texture[] causticTextures = new Texture[0]; // TODO populate from folder path

        //[Tooltip("Set shader globals for in shader caustics")]
        //public bool useShaderGlobals = true;

        //[Tooltip("Set cookie texture on light (old method)")]
        //public bool setCookieOnLight = false;


        [Space] [GUIColor("@dirlight ? Color.white : Color.red")] [InlineButton("FindSun")]
        public Light dirlight;

        private void FindSun()
        {
            dirlight = RenderSettings.sun;
        }

        //public List<VisualEffect> visualEffects = new List<VisualEffect>();
        [Space] public bool copyDirLightAngle = true;
        public Vector3 causticDirection = Vector3.down;

        [Space] public bool copyDirLightColor = false;
        public Color causticColor = Color.white;
        public float causticIntensityBase = 2.5f;
        public float causticIntensityExtra = 1.0f;

        [Tooltip("Pan caustics in direction, tranRotation follows the 'panDirFromTransform' rotation, tranWaveCenter points in the direction from this transform to 'panDirFromTransform' typically you would set the transform to the WaveCenter game object")]
        public enum eDirMethod
        {
            none,
            tranRotation,
            tranWaveCenter
        }

        [Space] public eDirMethod dirMethod = eDirMethod.tranRotation;

        [GUIColor("@panDirFromTransform ? Color.white : Color.red")] [InlineButton("FindWaveCenter", "Find")] [Tooltip("If set copies the transforms forward to set the caustics panning direction")]
        public Transform panDirFromTransform;

        public void FindWaveCenter()
        {
            var found = GameObject.Find("WaveCenter");
            if (found) panDirFromTransform = found.transform;
        }

        [Space] public float panSpeed = 0.5f;
        public Vector2 panDirection = Vector2.right;

        //public Vector2 causticPanSpeed = new Vector2(0.011875f, 0.053125f);


        [Space] [Tooltip("Don't use this, inverts the scale for caustics")]
        public bool flipCausticScale = false;

        public float causticScale = 0.2f;

        //public float causticBrightnessOffset = 0;
        //public float causticContrast = 1;

        public float fadeStart = 0;
        public float fadeEnd = 10;

        [Space(10)] public bool updateEveryFrame = false;
        [ShowInInspector] private double lastRan;
        public int fps = 30;

        //

        private int count;


        //

#if UNITY_EDITOR
        public void Reset()
        {
            panDirFromTransform = transform;
            name = "Caustics";
            FindTextures();
            FindSun();
        }
#endif

        private void OnEnable()
        {
            count = 0;
            lastRan = 0;
            if (dirlight == null) FindSun();
            PropertyUpdate();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update -= DisableLater;
            UnityEditor.EditorApplication.update += PropertyUpdateLater;
#endif
        }

        public void SetCausticColor(Color colorIn)
        {
            if (dirlight && copyDirLightColor) Debug.LogWarning("Cant set color copyDirLightColor is set", this);
            causticColor = colorIn;
            PropertyUpdate();
        }

        public void SetCausticIntensityMult(float valIn)
        {
            causticIntensityExtra = valIn;
            PropertyUpdate();
        }

        void OnValidate()
        {
            if (!isActiveAndEnabled) return;
            PropertyUpdate();
        }

        private Matrix4x4 lastTransform;

        void Update()
        {
            if (!isActiveAndEnabled) return;
            //StartCoroutine("TextureUpdate");   
            if (Time.realtimeSinceStartup - lastRan > 1.0f / fps)
            {
                TextureUpdate();
                if (debugCaustics)
                {
                    Debug.DrawLine(transform.position, transform.position + 3 * causticDirection, Color.white, 1.1f / fps);
                    Debug.DrawLine(transform.position, transform.position + 3 * new Vector3(panDirection.normalized.x, 0, panDirection.normalized.y), Color.red, 1.1f / fps);
                }
            }

            if (updateEveryFrame)
            {
                PropertyUpdate();
            }
            else
            {
                bool willUpdate = false;

                if (dirlight)
                {
                    if (copyDirLightAngle && causticDirection != dirlight.transform.forward) willUpdate = true;
                    if (copyDirLightColor && (causticColor != dirlight.color || causticIntensityBase != dirlight.intensity)) willUpdate = true;
                }

                if (panDirFromTransform)
                {
                    if (dirMethod == eDirMethod.tranRotation && panDirFromTransform.localToWorldMatrix != lastTransform)
                    {
                        lastTransform = panDirFromTransform.localToWorldMatrix;
                        willUpdate = true;
                    }

                    if (dirMethod == eDirMethod.tranWaveCenter && panDirFromTransform.localToWorldMatrix * transform.localToWorldMatrix != lastTransform)
                    {
                        lastTransform = panDirFromTransform.localToWorldMatrix * transform.localToWorldMatrix;
                        willUpdate = true;
                    }
                }


                if (willUpdate) PropertyUpdate();
            }
        }
/*
    IEnumerator TextureUpdate()
    {
        if (count == 100)
        {
            count = 0;
        }
        Dirlight.cookie = test[count];
        count = count + 1;
        yield return new WaitForSecondsRealtime (10f);
    }*/

        public void TextureUpdate()
        {
            lastRan = Time.realtimeSinceStartup;

            if (count >= causticTextures.Length) count = 0;

            if (causticTextures.Length <= 0) return;

            //if (useShaderGlobals)
            {
                Shader.SetGlobalTexture(CausticsPropName, causticTextures[count]);
            }

            //foreach (var visualEffect in visualEffects)
            //{
            //    if (visualEffect == null) continue;
            //    if (visualEffect.HasTexture("Caustics")) visualEffect.SetTexture("Caustics", test[count]);
            //}

            count = count + 1;
        }

        public void OnDisable()
        {
            SetDefaults();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update -= PropertyUpdateLater;
#endif
        }

        void OnDrawGizmos()
        {
            //if(debug)Gizmos.DrawLine(transform.position, transform.position + 3 * causticDirection);
        }

        [Button, ContextMenu("PropertyUpdate")]
        public void PropertyUpdate()
        {
#if UNITY_EDITOR
            if (debugCaustics && Application.isEditor) Debug.Log("Caustic: PropertyUpdate");
#endif

            if (dirlight)
            {
                //if (setCookieOnLight)
                //{
                //    dirlight.cookie = test[count]; // No caustics in URP till 2021.2
                //}

                if (copyDirLightColor)
                {
                    causticColor = dirlight.color;
                    causticIntensityBase = dirlight.intensity;
                }

                if (copyDirLightAngle)
                {
                    causticDirection = dirlight.transform.forward.normalized;
                }
            }

            if (panDirFromTransform)
            {
                switch (dirMethod)
                {
                    case eDirMethod.none:
                        break;
                    case eDirMethod.tranRotation:
                        var forward = panDirFromTransform.transform.forward;
                        panDirection = new Vector2(forward.x, forward.z).normalized;
                        break;
                    case eDirMethod.tranWaveCenter:
                        var forward2 = (transform.position - panDirFromTransform.transform.position).normalized;
                        panDirection = new Vector2(forward2.x, forward2.z).normalized;
                        break;
                }
            }

            //if (useShaderGlobals)
            {
                Shader.SetGlobalColor(CausticsColorPropName, causticColor * causticIntensityBase * causticIntensityExtra);
                //Shader.SetGlobalVector(CausticsDirPropName, new Vector4(causticDirection.x,causticDirection.y,causticDirection.z,0));
                //Shader.SetGlobalFloat(CausticsScalePropName, causticScale); // TODO send in the matrix?
                //Shader.SetGlobalFloat("_CausticsContrast", causticContrast);
                //Shader.SetGlobalFloat("_CausticsBrightnessOffset", causticBrightnessOffset);
                var panDirectionT = panDirection.normalized;
                //Shader.SetGlobalVector("_CausticsPanSpeed", new Vector4(panDirectionT.x * panSpeed, panDirectionT.y * panSpeed, 0, 0));
                Shader.SetGlobalVector(CausticsSettings, new Vector4(panDirectionT.x * panSpeed, panDirectionT.y * panSpeed, fadeStart, fadeEnd));
                //Shader.SetGlobalMatrix("_CausticMatrix",dirlight.transform.localToWorldMatrix);
                //if (dirlight && copyDirLightAngle)
                //{
                //    //var newmat = Matrix4x4.identity;
                //    //newmat.SetTRS(Vector3.zero, dirlight.transform.rotation, Vector3.one);
                //    //newmat.SetTRS(dirlight.transform.position, dirlight.transform.rotation, dirlight.transform.lossyScale);
                //    var newmat = dirlight.transform.localToWorldMatrix;
                //    Shader.SetGlobalMatrix("_CausticMatrix", newmat);
                //}
                //else

                Matrix4x4 cmatrix = Matrix4x4.identity;

                if (flipCausticScale)
                {
                    cmatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.LookRotation(Vector3.Normalize(causticDirection)), new Vector3(1.0f / causticScale, 1.0f / causticScale, 1.0f / causticScale));
                }
                else
                {
                    cmatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.LookRotation(Vector3.Normalize(causticDirection)), new Vector3(causticScale, causticScale, causticScale));
                }


                Shader.SetGlobalMatrix(CausticMatrix, cmatrix);
                //if (debug) Debug.Log("cmatrix: " + cmatrix);
                //Shader.SetGlobalVector("_CausticDistanceFade", new Vector4(fadeStart, fadeEnd, 0, 0));
            }
        }
    }
}
