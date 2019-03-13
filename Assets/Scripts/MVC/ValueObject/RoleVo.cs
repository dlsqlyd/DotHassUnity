using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotHassUnity
{
    /// <summary>
    /// 游戏角色的信息
    /// </summary>
    [Serializable]
    public class RoleVo
    {
        public long roleid { get; set; }
        public string name { get; set; }
        public string headid { get; set; }
        public int money { get; set; }
        public int gold { get; set; }
        public int exp { get; set; }

        public GangConfig gang { get; set; }
    }



    [Serializable]
    public class GangConfig
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Desc
        /// </summary>
        public string Desc { get; set; }
    }
}
