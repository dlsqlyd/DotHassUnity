#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace DotHass.Unity
{
    public static  class DeviceHelper
    {
      

        public static int ScreenWidth
        {
            get
            {
#if UNITY_EDITOR
                string[] res = UnityStats.screenRes.Split('x');
                return int.Parse(res[0]);
#else
            return Screen.width;
#endif
            }
        }

        public static int ScreenHeight
        {
            get
            {
#if UNITY_EDITOR
                string[] res = UnityStats.screenRes.Split('x');
                return int.Parse(res[1]);
#else
            return Screen.height;
#endif
            }
        }


        public static int  PlatformType
        {
            get{
                switch (UnityEngine.Application.platform)
                {
                    case RuntimePlatform.WindowsPlayer:
                        return 0;
                    case RuntimePlatform.IPhonePlayer:
                        return 4;
                    case RuntimePlatform.Android:
                        return 5;
                    default:
                        return 8;
                }
            }
        }

        /// <summary>
        /// ios7使用的是UIDevice identifierForVendor
        ///是给Vendor标识用户用的，每个设备在所属同一个Vender的应用里，都有相同的值。
        ///其中的Vender是指应用提供商，但准确点说，是通过BundleID的DNS反转的前两部分进行匹配，如果相同就是同一个Vender，
        ///例如对于com.somecompany.appone,com.somecompany.apptwo
        ///这两个BundleID来说，就属于同一个Vender，共享同一个idfv的值。
        ///和idfa不同的是，idfv的值是一定能取到的，所以非常适合于作为内部用户行为分析的主id，来标识用户，替代OpenUDID。
        ///注意：如果用户将属于此Vender的所有App卸载，则idfv的值会被重置，即再重装此Vender的App，idfv的值和之前不同。
        ///</summary>
        public static string UUID
        {
            get
            {
                return SystemInfo.deviceUniqueIdentifier;
            }
        }


    }
} 