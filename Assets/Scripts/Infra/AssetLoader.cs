using System;
using Cysharp.Threading.Tasks;
using Infra.Logging;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infra
{
    public static class AssetLoader
    {
        public static async UniTask<T> LoadAsync<T>(string address) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(address))
                throw new ArgumentException("Address cannot be null or empty", nameof(address));

            var handle = Addressables.LoadAssetAsync<T>(address);

            try
            {
                var asset = await handle.ToUniTask();
                if (asset == null)
                    LoggingFacade.LogError($"Asset at '{address}' was loaded but is null.");

                return asset;
            }
            catch (Exception ex)
            {
                LoggingFacade.LogError($"Failed to load asset '{address}': {ex}");
                return null;
            }
        }

        public static async UniTask<GameObject> InstantiateAsync(string address, Transform parent = null)
        {
            if (string.IsNullOrEmpty(address))
                throw new ArgumentException("Address cannot be null or empty", nameof(address));

            var handle = Addressables.InstantiateAsync(address, parent);

            try
            {
                var instance = await handle.ToUniTask();
                if (instance == null)
                    LoggingFacade.LogError($"Prefab '{address}' was instantiated but is null.");

                return instance;
            }
            catch (Exception ex)
            {
                LoggingFacade.LogError($"Failed to instantiate prefab '{address}': {ex}");
                return null;
            }
        }

        public static void Release<T>(T asset) where T : UnityEngine.Object
        {
            if (asset != null)
                Addressables.Release(asset);
        }
    }
}