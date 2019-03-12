using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotHass.Unity
{
    public class AlwaysIncludedShaders
    {
        [MenuItem("Tools/DotHass.Unity/Always included shader", false, 11)]
        public static void TestIncludedShader()
        {
            string[] myShaders = new string[]{
            };

            SerializedObject graphicsSettings = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset")[0]);
            SerializedProperty it = graphicsSettings.GetIterator();
            SerializedProperty dataPoint;
            while (it.NextVisible(true))
            {
                if (it.name == "m_AlwaysIncludedShaders")
                {
                    it.ClearArray();

                    for (int i = 0; i < myShaders.Length; i++)
                    {
                        it.InsertArrayElementAtIndex(i);
                        dataPoint = it.GetArrayElementAtIndex(i);
                        dataPoint.objectReferenceValue = Shader.Find(myShaders[i]);
                    }

                    graphicsSettings.ApplyModifiedProperties();
                }
            }
        }
    }
}
