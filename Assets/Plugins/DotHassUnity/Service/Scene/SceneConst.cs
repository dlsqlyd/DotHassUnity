using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFramework {

    /// <summary>
    /// 场景事件类
    /// </summary>
    public sealed class SceneConst
    {
        /// <summary>
        /// 场景切换事件
        /// </summary>
        public static string FlowScene { get; } = "FlowScene";


        // 进入场景
        public static string EnterScene { get; } = "EnterScene";


        // 退出场景
        public static string ExitScene { get; } = "ExitScene";

    }
}
