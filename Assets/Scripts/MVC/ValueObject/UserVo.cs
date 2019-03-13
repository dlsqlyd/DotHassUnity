using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotHassUnity
{
    /// <summary>
    /// 用细用户的信息
    /// </summary>
    [Serializable]
    public class UserVo
    {
        public string UserId { get; set; }

        public object RetailData { get; set; }

    }
}
