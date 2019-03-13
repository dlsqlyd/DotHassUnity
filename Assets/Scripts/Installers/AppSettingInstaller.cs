using DotHassUnity;
using DotHass.Unity;
using UnityEngine;
using Zenject;


namespace DotHassUnity
{

    [CreateAssetMenu(fileName = "AppSettingInstaller", menuName = "Installers/AppSettingInstaller")]
    public class AppSettingInstaller : ScriptableObjectInstaller<AppSettingInstaller>
    {

        public AppSetting appSetting;



        public override void InstallBindings()
        {
            Container.BindInstances(appSetting);
            //TODO:������Ҫ��Щ���õ���


        }
    }
}