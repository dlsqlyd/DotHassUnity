using DotHass.Unity;
using DotHass.Unity.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace DotHassUnity
{
    [Serializable]
    public class AppSetting
    {
        public string name = "DotHass Unity";

        public bool Debug = true;

        public string platform = "0000";

        public int GameType = 1;

        public int ServerID = 1;

        public float globalVolume = 1;

        public List<ChannelOptions> options;


        private static AppSetting _instance;


        public static AppSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = App.ProjectContainer.Resolve<AppSetting>();
                }

                return _instance;
            }
        }
    }
}
