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
        /// ios7ʹ�õ���UIDevice identifierForVendor
        ///�Ǹ�Vendor��ʶ�û��õģ�ÿ���豸������ͬһ��Vender��Ӧ���������ͬ��ֵ��
        ///���е�Vender��ָӦ���ṩ�̣���׼ȷ��˵����ͨ��BundleID��DNS��ת��ǰ�����ֽ���ƥ�䣬�����ͬ����ͬһ��Vender��
        ///�������com.somecompany.appone,com.somecompany.apptwo
        ///������BundleID��˵��������ͬһ��Vender������ͬһ��idfv��ֵ��
        ///��idfa��ͬ���ǣ�idfv��ֵ��һ����ȡ���ģ����Էǳ��ʺ�����Ϊ�ڲ��û���Ϊ��������id������ʶ�û������OpenUDID��
        ///ע�⣺����û������ڴ�Vender������Appж�أ���idfv��ֵ�ᱻ���ã�������װ��Vender��App��idfv��ֵ��֮ǰ��ͬ��
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