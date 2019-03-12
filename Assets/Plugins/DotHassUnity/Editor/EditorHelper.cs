using UnityEngine;
using UnityEditor;
using System.IO;

namespace HFramework
{

    public static class ScriptableObjectUtility
    {
        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        public static void CreateAsset<T>(string name = "") where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            if (string.IsNullOrEmpty(name))
            {
                name = "New " + typeof(T).ToString();
            }
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + name + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }

    public class EditorHelper
    {

        /// <summary>
        /// Create Folder(path not include "Assets")
        /// EX: GameResources/Prefabs/Sprites/Enemy
        /// </summary>
        public static void CreateFolder(string name)
        {
            string[] splitName = name.Split('/');

            string prefixFolderName = "";
            string pathValid = "";
            for (int i = 0; i < splitName.Length; i++)
            {
                pathValid += "/" + splitName[i];

                if (AssetDatabase.IsValidFolder("Assets" + pathValid) == false)
                    AssetDatabase.CreateFolder("Assets" + prefixFolderName, splitName[i]);

                prefixFolderName += "/" + splitName[i];
            }
        }


        public static string AssetPathToFilePath(string projectPath)
        {
            return Application.dataPath.Replace("Assets", "") + projectPath;
        }

        public static string FilePathToAssetPath(string FilePath)
        {
            return FilePath.Replace("\\", "/").Replace(Application.dataPath, "Assets");
        }


        public static string GetScriptPath(ScriptableObject o)
        {
            MonoScript ms = MonoScript.FromScriptableObject(o);
            var m_ScriptFilePath = AssetDatabase.GetAssetPath(ms);
            var m_ScriptFolder = Path.GetDirectoryName(m_ScriptFilePath);

            return m_ScriptFolder;
        }

        public static string GetScriptPath(string name)
        {
            string[] assets = AssetDatabase.FindAssets("t:script " + name);
            if (assets.Length == 0)
            {
                Debug.LogError(name + " not found, make sure you have the Gamestrap scripts in your project.");
                return null;
            }
            string path = AssetDatabase.GUIDToAssetPath(assets[0]);
            DirectoryInfo dir = Directory.GetParent(path);
            return "Assets" + dir.Parent.FullName.Substring(UnityEngine.Application.dataPath.Length) + "\\";
        }

    }



}
