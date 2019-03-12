using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace HFramework
{
    /// <summary>
    /// 戏中许多时候会使用一个透明的Image组件来监听点击事件或者屏蔽Image后面的按钮事件，
    /// 空的Image可以解决这个问题，用起来也很方便，但是空的Image照旧会参与绘制，
    /// 从而产生overdraw。解决办法是扩展Graphic组件来替换Image组件。 
    /// 如果是只要点击区域，不要显示内容的。
    /// 可以把空白透明Image替换成qiankanglai提供的Empty4Raycast （http://blog.uwa4d.com/archives/fillrate.html）。
    /// 只接收事件，清空顶点绘制。
    /// </summary>
    public class Empty4Raycast : MaskableGraphic
    {
        protected Empty4Raycast()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}
