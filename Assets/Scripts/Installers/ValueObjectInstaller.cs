using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace DotHassUnity
{
    public class ValueObjectInstaller : Installer<ValueObjectInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<UserVo>().AsSingle();
            Container.Bind<RoleVo>().AsSingle();
        }
    }
}
