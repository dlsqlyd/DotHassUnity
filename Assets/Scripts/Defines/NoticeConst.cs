using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace DotHassUnity
{

    public class NoticeConst
    {

        #region application 或者service
        public static string Startup { get; } = "E_Startup";    // 启动游戏
        public static string Shutdown { get; } = "E_Shutdown";  // 退出游戏

        public static string Restart { get; } = "E_Restart";  // 退出游戏
        #endregion



        #region command监听的事件

        public const string PassportEnter = "PassportCommandEnter";
        public const string PassportExit = "PassportCommandExit";


        public const string HomeEnter = "HomeCommandEnter";
        public const string HomeExit = "HomeCommandExit";


        #endregion


        #region enter

        public static string LOGIN_FAILED { get; } = "LOGIN_FAILED";
        public static string LOGIN_SUCCESS { get; } = "LOGIN_SUCCESS";
        public static string REG_FAILED { get; } = "REG_FAILED";
        public static string REG_SUCCESS { get; } = "REG_SUCCESS";

        #endregion






    }
}
