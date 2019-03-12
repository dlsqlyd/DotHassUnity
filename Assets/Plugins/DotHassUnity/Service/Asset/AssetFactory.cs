using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace HFramework
{
    public class AssetFactory: IFactory<AssetLoader>
    {
        private DiContainer _container;

        public AssetFactory(DiContainer container)
        {
            _container = container;
       
        }
        public  AssetLoader Create()
        {
            return _container.Instantiate<AssetLoader>(new object[] {
           
            });
        }
    }

}
