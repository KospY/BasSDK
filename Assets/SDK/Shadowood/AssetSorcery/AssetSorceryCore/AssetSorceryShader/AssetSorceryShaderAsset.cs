using System;
using System.Collections.Generic;
using System.Linq;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEditor;
using UnityEngine;

namespace ThunderRoad.AssetSorcery
{
    [Serializable]
    public class AssetSorceryShaderAsset : AssetSorceryAssetCommon<AFilterCommon<Shader>, Shader>
    {
        public List<Shader> replaceShaders = new List<Shader>();
        
        public Shader targetShader;


        //[MultiLineProperty(6)] public string mali;

        public bool debug;


        public AssetSorceryShaderSettings shaderSettings = new AssetSorceryShaderSettings();

        //

        public override void DrawCustomInspector(SerializedObject extraDataSerializedObject)
        {
            //SerializedProperty textureProps = extraDataSerializedObject.FindProperty("textureSettings");
            //EditorGUILayout.PropertyField(textureProps);
        }

        public override List<AFilterCommon<Shader>> GetFilterEntries()
        {
            return new List<AFilterCommon<Shader>>
            {
                new FilterPlatform<Shader>
                {
                    item = targetShader
                }
            };
        }

        public override AFilterCommon<Shader> ReturnFilterMatch()
        {
            return new FilterPlatform<Shader>
            {
                item = targetShader
            };
        }

        //
        //
        //

        public static T ParseEnum<T>(string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }

        public List<PassItem> FindAllPasses(List<PassItem> listIn, Shader shader, string source, bool debug = false)
        {
            var result = listIn;

            //listIn.Clear();

            foreach (var o in result)
            {
                o.present = false;
            }

            //var lines = source.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None).ToList();

            //lines = lines.Where((value) => value.Contains("Name \"")).ToList();
            //lines = lines.Where((value) => !value.TrimStart().StartsWith("//")).ToList();

            //Debug.Log("FindAllPasses: " + shader.name);

            var shaderInfo = ShaderUtil.GetShaderInfo(shader);

            var shaderData = ShaderUtil.GetShaderData(shader);


            for (int i = 0; i < shaderData.SerializedSubshaderCount; i++)
            {
                //if (index >= 0 && index < this.SerializedSubshaderCount)
                //if (i + 1 > shaderData.SubshaderCount) break;
                ShaderData.Subshader subShader = null;
                try
                {
                    subShader = shaderData.GetSerializedSubshader(i);
                }
                catch (Exception e)
                {
                    // ignored
                    continue;
                }

                if (subShader != null)
                {
                    //Debug.Log("FindAllPasses: SubShader: " + i);

                    for (int j = 0; j < subShader.PassCount; j++)
                    {
                        var pass = subShader.GetPass(j);

                        //Debug.Log("FindAllPasses: Pass: " + pass.Name + ":" + pass.SourceCode.Length);

                        PassItem selectedPass = null;

                        bool found = false;
                        foreach (var passItem in listIn)
                        {
                            if (passItem.passName == pass.Name)
                            {
                                selectedPass = passItem;
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            selectedPass = new PassItem
                            {
                                passName = pass.Name,
                                present = true,
                                enabledMobile = true,
                                enabledDesktop = true
                            };
                            listIn.Add(selectedPass);
                        }

                        if (selectedPass != null)
                        {
                            selectedPass.indexSubShader = i;
                            //selectedPass.indexPass = j;
                            selectedPass.present = true;
                        }
                    }
                }
            }
            /*
            SerializedObject so = new SerializedObject (shader);
            SerializedProperty subShaders = so.FindProperty ("m_ParsedForm.m_SubShaders");
            for (int k = 0; k < subShaders.arraySize; k++) {
                Debug.Log ("FindAllPasses: SubShader: " + k);
                SerializedProperty subShader = subShaders.GetArrayElementAtIndex (k);
                SerializedProperty passes = subShader.FindPropertyRelative ("m_Passes");
                for (int i = 0; i < passes.arraySize; i++) {
                    Debug.Log ("FindAllPasses: Pass: " + i);
                    SerializedProperty pass = passes.GetArrayElementAtIndex (i);
 
                    SerializedProperty name = pass.FindPropertyRelative("m_State.m_Name");
                    Debug.Log("FindAllPasses: Name: " + name.stringValue);
                    
                    listIn.Add(new PassItem(){ passName = name.stringValue});
 
                    SerializedProperty tags = pass.FindPropertyRelative ("m_State.m_Tags.tags");
                    for (int j = 0; j < tags.arraySize; j++) {
                        SerializedProperty tag = tags.GetArrayElementAtIndex (j);
                        SerializedProperty first = tag.FindPropertyRelative ("first");
                        SerializedProperty second = tag.FindPropertyRelative ("second");
                        Debug.Log (first.stringValue + ":" + second.stringValue);
                    }
                }
            }*/

            foreach (var o in result)
            {
                o.DisplayNameCalc();
            }

            return listIn;
        }

        public List<AssetSorceryShaderKeyword> FindAllKeywords(List<AssetSorceryShaderKeyword> listIn, string source, bool debug = false)
        {
            var result = listIn;

            //bool debug = false;

            foreach (var siSshaderKeyword in result)
            {
                siSshaderKeyword.present = false;
            }

            var lines = source.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None).ToList();

            lines = lines.Where((value) => value.Contains("#pragma")).ToList();
            lines = lines.Where((value) => !value.TrimStart().StartsWith("//")).ToList();

            var domains = Enum.GetNames(typeof(AssetSorceryShaderKeyword.eKeywordDomain));
            var keywords = Enum.GetNames(typeof(AssetSorceryShaderKeyword.eKeywordType));

            foreach (var line2 in lines)
            {
                var line = line2.TrimStart().TrimEnd();

                foreach (var type in keywords)
                {
                    if (debug) Debug.Log(line + ":" + type + ":" + line.Contains(type));
                    if (!line.Contains(type)) continue;
                    foreach (var domain in domains)
                    {
                        var domain2 = domain;
                        if (domain == AssetSorceryShaderKeyword.eKeywordDomain.none.ToString()) domain2 = "";

                        bool fog = false;
                        if (domain == AssetSorceryShaderKeyword.eKeywordDomain.none.ToString() && type == AssetSorceryShaderKeyword.eKeywordType.multi_compile_fog.ToString())
                        {
                            fog = line.Contains(AssetSorceryShaderKeyword.eKeywordType.multi_compile_fog.ToString());
                        }

                        if (line.Contains(type + domain2 + " ") || line.Contains(type + "_local" + domain2 + " ") || fog)
                        {
                            bool isLocal = line.Contains(type + "_local");
                            var localness = "";
                            if (isLocal) localness = "_local";

                            var type2 = type.Replace("_local", "");

                            var keyType = ParseEnum<AssetSorceryShaderKeyword.eKeywordType>(type2);
                            var keyDomain = ParseEnum<AssetSorceryShaderKeyword.eKeywordDomain>(domain);
                            var split = line.Split(new[] {type + localness + domain2 + " "}, StringSplitOptions.None);


                            var keywordEnd = line;
                            var keywordLast = line;
                            if (split.Length > 1) keywordLast = keywordEnd = split[1];

                            var split2 = keywordLast.Split(' ');
                            if (split2.Length > 1) keywordLast = split2.Last();
                            if (keywordLast.Contains("_ON")) keywordLast = keywordLast.Replace("_ON", "");

                            if (debug) Debug.Log("Found: " + line + "---" + type + ":" + domain + ":" + isLocal);

                            AssetSorceryShaderKeyword foundItem = new AssetSorceryShaderKeyword();

                            bool alreadyExists = false;
                            foreach (var ss in result)
                            {
                                //if (ss.keywordType == keyType && ss.keywordDomain == keyDomain && ss.local == isLocal && ss.keywordName == keywordEnd)
                                //if (ss.keywordName == keywordEnd)
                                if (ss.keywordLast == keywordLast)
                                {
                                    //ss.present = true;
                                    alreadyExists = true;
                                    foundItem = ss;
                                    break;
                                }
                            }

                            if (!alreadyExists)
                            {
                                foundItem = new AssetSorceryShaderKeyword
                                {
                                    enabledDesktop = true,
                                };
                                result.Add(foundItem);
                            }
                            else
                            {
                                if (debug) Debug.Log("Found: skipping: " + line);
                            }

                            var keyDomainText = " : " + keyDomain.ToString();
                            if (keyDomainText.Contains("_")) keyDomainText = keyDomainText.Replace("_", " ");

                            var keyTypeText = keyType.ToString();
                            if (keyTypeText.Contains("_")) keyTypeText = keyTypeText.Replace("_", " ");

                            if (keyDomain == AssetSorceryShaderKeyword.eKeywordDomain.none) keyDomainText = "";

                            //string keywordEndText = keywordEnd;
                            //if (keywordEndText.Contains("__ ")) keywordEndText = keywordEndText.Replace("__ ", "");
                            //if (keywordEndText.Contains("_ ")) keywordEndText = keywordEndText.Replace("_ ", "");
                            //if (keywordEndText.Contains("_")) keywordEndText = keywordEndText.Replace("_", " ");
/*
                            string instructionsCount = "";
                            if (foundItem.instructions != 0)
                            {
                                instructionsCount = " (" + foundItem.instructions.ToString() + ")";
                            }

                            var prefix = (foundItem.present ? "" : " -- ") + (foundItem.enabledDesktop ? "ON - " : "OFF - ") +  (foundItem.enabledMobile ? "ON - " : "OFF - ");
                            if (foundItem.enabledDesktop && !string.IsNullOrEmpty(foundItem.replace)) prefix = "REP - ";
                            foundItem.displayName = prefix + keywordEndText + " : " + keyTypeText + keyDomainText + instructionsCount;
                            */
                            foundItem.line = line;
                            foundItem.keywordName = keywordEnd;
                            foundItem.keywordLast = keywordLast;
                            foundItem.keywordType = keyType;
                            foundItem.keywordDomain = keyDomain;
                            foundItem.local = isLocal;
                            foundItem.present = true;
                        }
                    }
                }
            }

            foreach (var assetSorceryShaderKeyword in result)
            {
                assetSorceryShaderKeyword.DisplayNameCalc();
            }

            if (debug) Debug.Log("Found: End Count: " + result.Count);
            return result;
        }
    }
}
