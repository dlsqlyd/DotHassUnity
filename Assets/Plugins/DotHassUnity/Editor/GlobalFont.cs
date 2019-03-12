using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HFramework
{
    public class GlobalFont
    {
        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            EditorApplication.hierarchyChanged += ChangeDefaultFont;
        }

        private static void ChangeDefaultFont()
        {
            if (Selection.activeGameObject == null)
            {
                return;
            }
            Text text = Selection.activeGameObject.GetComponent<Text>();
            if (text != null)
            {
                var data = FontDataManager.GetFont();
                text.font = data.font;
                text.fontSize = data.fontSize;
                text.fontStyle = data.fontStyle;
                text.color = data.color;
            }
        }
    }


    [System.Serializable]
    public class FontData : ScriptableObject
    {
        [SerializeField]
        public Font font;

        public FontStyle fontStyle;
        public int fontSize;

        public Color color;

    }
    public class FontDataManager
    {
        public static string path = "Assets/Editor/DefaultFontData.asset";



        /// <summary>
        /// CreateAssetMenu 主要是应用在类上
        /// MenuItem主要是应用在方法上。
        /// </summary>

        [MenuItem("Assets/Create/FontData")]
        public static void CreateMyAsset()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path.Contains("Editor") == false)
            {
                UnityEditor.EditorUtility.DisplayDialog("Title", "必须放在editor目录下", "ok");
                return;
            }

            ScriptableObjectUtility.CreateAsset<FontData>("DefaultFontData");
        }

        public static void SaveFont(Font font)
        {
            FontData data = ScriptableObject.CreateInstance<FontData>();
            data.font = font;
            AssetDatabase.CreateAsset(data, path);
        }

        public static FontData GetFont()
        {
            return AssetDatabase.LoadAssetAtPath<FontData>(path);
        }
    }
}
