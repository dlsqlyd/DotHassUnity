using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HFramework
{
    /// <summary>
    /// 复制目录下的文件名为字符窜到剪贴板..
    /// </summary>
    public class CopyFileNameUtils:Editor
    {
        [MenuItem("Assets/CopyFileNames/AllDirectories", false ,900)]
        public static void AllDirectories()
        {
            CopyFileNames(SearchOption.AllDirectories);
        }
   
        [MenuItem("Assets/CopyFileNames/AllDirectories", true)]
        static bool ValidateAllDirectories()
        {
            return CheckIsDirectory();
        }


        [MenuItem("Assets/CopyFileNames/TopDirectoryOnly", false, 900)]
        public static void TopDirectoryOnly()
        {
            CopyFileNames(SearchOption.TopDirectoryOnly);
        }

        [MenuItem("Assets/CopyFileNames/TopDirectoryOnly", true)]
        static bool ValidateTopDirectoryOnly()
        {
            return CheckIsDirectory();
        }

        [MenuItem("Assets/CopyFileNames/FilePath", false, 900)]
        public static void FilePath()
        {
            string filePath = AssetDatabase.GetAssetPath(Selection.activeObject);
            filePath = EditorHelper.AssetPathToFilePath(filePath);
            var result = GetFile(filePath);
            Debug.Log("已经复制到剪贴板:\n" + result);
            TextEditor textEditor = new TextEditor();
            textEditor.text = result;
            textEditor.OnFocus();
            textEditor.Copy();
        }

        [MenuItem("Assets/CopyFileNames/FilePath", true)]
        static bool ValidateFilePath()
        {
            string filePath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            FileAttributes attr = File.GetAttributes(filePath);
            return ((attr & FileAttributes.Directory) != FileAttributes.Directory);
        }


        static bool CheckIsDirectory()
        {
            string filePath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }
            FileAttributes attr = File.GetAttributes(filePath);
            return ((attr & FileAttributes.Directory) == FileAttributes.Directory);
        }


        static void CopyFileNames(SearchOption searchOption)
        {
            string filePath = AssetDatabase.GetAssetPath(Selection.activeObject);

            filePath = EditorHelper.AssetPathToFilePath(filePath);

            var files = new DirectoryInfo(filePath).GetFiles("*", searchOption);
            var result = String.Join(",\n ", files.Where((s) => Path.GetExtension(s.Name) != ".meta").Select((s) => "\""+ GetFile(s.FullName) + "\"").ToArray());

            Debug.Log("已经复制到剪贴板:\n" + result);

            TextEditor textEditor = new TextEditor();
            textEditor.text = result;
            textEditor.OnFocus();
            textEditor.Copy();
        }


        static string GetFile(string filePath)
        {
            return new DirectoryInfo(Path.GetDirectoryName(filePath)).Name + "/" + Path.GetFileNameWithoutExtension(filePath);
        }

    }
}
