using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.U2D;
using static UnityEngine.AddressableAssets.Addressables;
using Object = UnityEngine.Object;

namespace DotHass.Unity
{
    public class AssetService : IAssetService
    {
        public IAsyncOperation<GameObject> Instantiate(object key, InstantiationParameters instantiateParameters)
        {
            return Addressables.Instantiate(key, instantiateParameters);
        }

        public IAsyncOperation<GameObject> Instantiate(object key, Transform parent = null, bool instantiateInWorldSpace = false)
        {
            return Addressables.Instantiate(key, parent, instantiateInWorldSpace);
        }


        public IAsyncOperation<GameObject> Instantiate(object key, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return Addressables.Instantiate(key, position, rotation, parent);
        }


        public IAsyncOperation<IList<GameObject>> InstantiateAll(object key, Action<IAsyncOperation<GameObject>> callback, InstantiationParameters instantiateParameters)
        {
            return Addressables.InstantiateAll(key, callback, instantiateParameters);
        }
        public IAsyncOperation<IList<GameObject>> InstantiateAll(object key, Action<IAsyncOperation<GameObject>> callback, Transform parent = null, bool instantiateInWorldSpace = false)
        {
            return Addressables.InstantiateAll(key, callback, parent, instantiateInWorldSpace);
        }

        public IAsyncOperation<TObject> LoadAsset<TObject>(object key) where TObject : class
        {
            return Addressables.LoadAsset<TObject>(key);
        }

        public IAsyncOperation<IList<TObject>> LoadAssets<TObject>(IList<object> keys, Action<IAsyncOperation<TObject>> callback, Addressables.MergeMode mode = Addressables.MergeMode.None) where TObject : class
        {
            return Addressables.LoadAssets<TObject>(keys, callback, mode);
        }
        public IAsyncOperation<IList<TObject>> LoadAssets<TObject>(object key, Action<IAsyncOperation<TObject>> callback) where TObject : class
        {
            return Addressables.LoadAssets<TObject>(key, callback);
        }


        public void ReleaseAsset<TObject>(TObject asset) where TObject : class
        {
            Addressables.ReleaseAsset<TObject>(asset);
        }

        public void ReleaseInstance(GameObject instance, float delay = 0)
        {
            Addressables.ReleaseInstance(instance, delay);
        }


        public IAsyncOperation DownloadDependencies(IList<object> keys, MergeMode mode)
        {
            return Addressables.DownloadDependencies(keys, mode);
        }
        public IAsyncOperation DownloadDependencies(object key)
        {
            return Addressables.DownloadDependencies(key);
        }
    }


    public static class IAsyncOperationExtensions
    {
        public static AsyncOperationAwaiter GetAwaiter(this IAsyncOperation operation)
        {
            return new AsyncOperationAwaiter(operation);
        }

        public static AsyncOperationAwaiter<T> GetAwaiter<T>(this IAsyncOperation<T> operation)
        {
            return new AsyncOperationAwaiter<T>(operation);
        }

        public struct AsyncOperationAwaiter : INotifyCompletion
        {
            private readonly IAsyncOperation _operation;

            public AsyncOperationAwaiter(IAsyncOperation operation)
            {
                _operation = operation;
            }

            public bool IsCompleted => _operation.Status != AsyncOperationStatus.None;

            public void OnCompleted(Action continuation) => _operation.Completed += (op) => continuation?.Invoke();

            public object GetResult() => _operation.Result;
        }

        public struct AsyncOperationAwaiter<T> : INotifyCompletion
        {
            private readonly IAsyncOperation<T> _operation;

            public AsyncOperationAwaiter(IAsyncOperation<T> operation)
            {
                _operation = operation;
            }

            public bool IsCompleted => _operation.Status != AsyncOperationStatus.None;

            public void OnCompleted(Action continuation) => _operation.Completed += (op) => continuation?.Invoke();

            public T GetResult() => _operation.Result;
        }
    }
}
