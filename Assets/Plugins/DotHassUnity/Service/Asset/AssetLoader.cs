using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System;
using Object = UnityEngine.Object;
using Newtonsoft.Json;
using UnityEngine.ResourceManagement;
using System.Collections.Concurrent;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HFramework
{

    /// <summary>
    /// 负责维护一个加载列表
    /// AssetService 只负责基本的加载
    /// </summary>
    public class AssetLoader: IAssetLoader
    {

        /// <summary>
        /// 可能同时使用LoadAsync()和LoadAsync<TObject>(object key)
        /// 问题出现再spriteatlasservice。。所以这里使用ConcurrentDictionary
        /// </summary>
        public ConcurrentDictionary<string, UnityEngine.Object> Links = new ConcurrentDictionary<string, UnityEngine.Object>();
        protected AssetService assetService;
        protected List<string> keys;

        public AssetLoader(AssetService service, List<string> keys)
        {
            this.assetService = service;
            this.keys = keys;
        }




        /// <summary>
        ///  实例化一个gameobject并添加组件
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <param name="worldPositionStays"></param>
        /// <returns></returns>
        public TObject Instantiate<TObject>(string key, Transform parent = null, bool worldPositionStays = false) where TObject : Component
        {
          return this.Instantiate(key, parent, worldPositionStays).GetOrAddComponent<TObject>();
        }

        /// <summary>
        /// 实例化一个gameobject
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <param name="worldPositionStays"></param>
        /// <returns></returns>
        public GameObject Instantiate(string key, Transform parent = null, bool worldPositionStays = false)
        {
            return UnityEngine.Object.Instantiate<GameObject>(this.LoadAsset<GameObject>(key), parent, worldPositionStays);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public TObject LoadJsonAsset<TObject>(string key) where TObject : class
        {
            return JsonConvert.DeserializeObject<TObject>(this.LoadAsset<TextAsset>(key).text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public TObject LoadAsset<TObject>(string key) where TObject : class
        {
            if (this.Links.TryGetValue(key, out UnityEngine.Object obj))
            {
                return obj as TObject;
            }
            UnityEngine.Debug.Log($"你加载的资源key:{key}不存在");
            return default(TObject);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TObject> LoadNameAsync<TObject>(string key) where TObject : class
        {
            if(Links.ContainsKey(key))
            {
                return Links[key] as TObject;
            }
            var asset= await assetService.LoadAsset<Object>(key);
            Links.TryAdd(key, asset);
            return asset as TObject;
        }



        public virtual async Task LoadKeysAsync()
        {
            foreach (var key in keys)
            {
                await this.LoadLableOrName(key);
            }
        }


        protected async Task LoadLableOrName(string key)
        {
            if(this.Links.ContainsKey(key))
            {
                return;
            }
            await assetService.LoadAssets<Object>(key, (IAsyncOperation<Object> asset)=> {
                var assetKey = asset.Context.ToString();
                if (this.Links.ContainsKey(assetKey))
                {
                    return;
                }
                Links.TryAdd(assetKey, asset.Result);
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void ReleaseAsset(string key)
        {
            if (this.Links.TryGetValue(key, out UnityEngine.Object obj))
            {
                this.Links.TryRemove(key, out _);
                this.assetService.ReleaseAsset<UnityEngine.Object>(obj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReleaseAll()
        {
            var keys = Links.Keys.ToList();

            for (int i = keys.Count - 1; i >= 0; i--)
            {
                ReleaseAsset(keys[i]);
            }
            Links.Clear();
        }
    }

}
