using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotHassUnity
{
    public sealed class AppConst
    {
        public const string EnterSceneName = "EnterScene";
        public const string MainSceneName = "MainScene";


        #region UI 结构
        public static string RootName { get; } = "UI";
        public static string CameraName { get; } = "Camera";
        public static string MainName { get; } = "Main";
        public static string PopName { get; } = "Pop";
        public static string FloatingName { get; } = "Floating";

        public const string RootId = "UIRoot";
        public const string CameraId = "UICamera";
        public const string MainId = "UIMain";
        public const string PopId = "UIPop";
        public const string FloatingId = "UIFloating";
        #endregion




        #region platform
        public static String DefaultPlatform { get; } = "0000";
        public static String IosPlatform { get; } = "0001";
        #endregion




    }
}
