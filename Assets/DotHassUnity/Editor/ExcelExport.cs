using DotHass.Unity;
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace DotHass.Unity
{
    public class ExcelExport : Editor
    {

        public static string exeFilePath = Application.dataPath + "\\..\\..\\GYJServer\\dothass\\DotHass.Tools.ExcelExport\\bin\\Release\\netcoreapp2.2\\publish\\DotHass.Tools.ExcelExport.exe";

        [MenuItem("Tools/ExcelExport/Config", false, 40)]
        public static void ExportConfig()
        {

            string param0 = "--ShowInfo false";

            string param1 = "--ExcelDir Configs";

            string param2 = "--JsonDir " + Application.dataPath + "\\_Assets\\Designer\\Config";

            try
            {
                Process myProcess = new Process();
                myProcess.StartInfo.FileName = exeFilePath;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.Arguments = param0 + " " + param1 + " " + param2;
                myProcess.EnableRaisingEvents = true;

                UnityEngine.Debug.Log(exeFilePath + "  " + myProcess.StartInfo.Arguments);
                myProcess.Start();
                myProcess.WaitForExit();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e);
            }
        }


        /// <summary>
        /// F:/GYJ/Source/GouYuJian/Assets\..\..\..\Tools\ExcelExport\DotHass.Tools.ExcelExport.exe  --ShowInfo false --ExcelDir Configs --CSharpDir  F:/GYJ/Source/GouYuJian/Assets\Scripts\Application\Configs --CSharpTemplate F:/GYJ/Source/GouYuJian/Assets\_Assets\Templates\ExcelExportCSharpTemplate.txt
        /// </summary>
        [MenuItem("Tools/ExcelExport/CSharp", false, 40)]
        public static void ExportCSharp()
        {

            string param0 = "--ShowInfo false";

            string param1 = "--ExcelDir Configs";

            string param2 = "--CSharpDir  " + Application.dataPath + "\\Scripts\\Configs";

            string param3 = "--CSharpTemplate  " + Application.dataPath + "\\_Assets\\Templates\\ExcelExportCSharpTemplate.txt";



            try
            {
                Process myProcess = new Process();
                myProcess.StartInfo.FileName = exeFilePath;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.Arguments = param0 + " " + param1 + " " + param2 + " " + param3;
                myProcess.EnableRaisingEvents = true;

                UnityEngine.Debug.Log(exeFilePath + "  " + myProcess.StartInfo.Arguments);
                myProcess.Start();
                myProcess.WaitForExit();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e);
            }
        }




        [MenuItem("Tools/ExcelExport/Map", false, 40)]
        public static void ExportMap()
        {
            string param0 = "--ShowInfo false";

            string param1 = "--ExcelDir Maps";

            string param2 = "--JsonDir " + Application.dataPath + "\\_Assets\\Designer\\Map";

            string param3 = "--FilterRow false";

            string param4 = "--FilterCol false";

            try
            {
                Process myProcess = new Process();
                myProcess.StartInfo.FileName = exeFilePath;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.Arguments = param0 + " " + param1 + " " + param2 + " " + param3 + " " + param4;
                myProcess.EnableRaisingEvents = true;

                UnityEngine.Debug.Log(exeFilePath + "  " + myProcess.StartInfo.Arguments);
                myProcess.Start();
                myProcess.WaitForExit();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e);
            }

        }
    }


}