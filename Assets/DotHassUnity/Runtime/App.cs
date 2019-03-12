
using PureMVC.Interfaces;
using PureMVC.Patterns.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace DotHass.Unity
{
    public sealed class App
    {
        public static DiContainer ProjectContainer
        {
            get
            {
                return ProjectContext.Instance.Container;
            }
        }


        //不要在任何类中保存SceneContainer,因为SceneContainer在切换场景后会被重置
        public static DiContainer SceneContainer { get; set; }

        public const string FacadeDefaultKey = "AppFacade";


        //facade不注册到container中,因为重启游戏的时候得重新生成一个新的facade
        //而facade如果被注入到对象后..即使重新bind.但是注入过的facade改变不了
        //不要在任何类中保存Facade,因为facade在重启游戏后会被重置
        private static Facade _facade;
        public static Facade Facade
        {
            get
            {
                if (_facade == null)
                {
                    _facade = new Facade(FacadeDefaultKey);
                }
                return _facade;
            }
            set
            {
                _facade = value;
            }
        }

        public static void ResetFacade()
        {
            Facade.RemoveCore(App.FacadeDefaultKey);
            Facade = null;
        }
    }

}
