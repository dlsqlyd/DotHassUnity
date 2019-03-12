
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HFramework
{
    public class AssetLoaderProgress:AssetLoader
    {
        public Action<float> OnProgress { get; set; }
        public Action OnComplete { get; set; }

        public AssetLoaderProgress(AssetService service, List<string> setting):base(service,setting)
        {
        }

        public override async Task LoadKeysAsync()
        {
            var total = keys.Count;

            for (int i = 0; i < total; i++)
            {
                var key = keys[i];
                await this.LoadLableOrName(key);
                if (OnProgress != null)
                {
                    OnProgress((i+1) / total);
                }
            }

            if (OnComplete != null)
            {
                OnComplete();
            }
        }


    }
}
