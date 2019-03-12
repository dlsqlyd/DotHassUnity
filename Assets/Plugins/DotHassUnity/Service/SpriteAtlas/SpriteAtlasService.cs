using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using Zenject;

namespace HFramework
{
    public class SpriteAtlasService : IAssetService
    {

        public const string ASSETLOADID = "SpriteAtlasId";
        private string pathroot;
        private AssetLoader assetLoader;
        private string extension = ".spriteatlas";

        public SpriteAtlasService([Inject(Id = "SpriteAtlasId")] AssetLoader assetLoader, string pathroot)
        {
            SpriteAtlasManager.atlasRequested += RequestAtlas;
            this.pathroot = pathroot;
            this.assetLoader = assetLoader;
        }

        private string getFullPath(string name) => (this.pathroot + name + this.extension);

        public Sprite GetSprite(string name, string sprite) => this.assetLoader.LoadAsset<SpriteAtlas>(this.getFullPath(name)).GetSprite(sprite);

        public async void RequestAtlas(string name, Action<SpriteAtlas> callback)
        {
            var sa = await assetLoader.LoadNameAsync<SpriteAtlas>(this.getFullPath(name));
            callback(sa);
        }

        public void ReleaseAsset(string name)
        {
            this.assetLoader.ReleaseAsset(this.getFullPath(name));
        }

    }
}
