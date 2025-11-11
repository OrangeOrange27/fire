using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Infra
{
    public static class AddressablesSceneLoader
    {
        public static async UniTask LoadSceneAsync(string sceneKey, LoadSceneMode mode = LoadSceneMode.Single)
        {
            var handle = Addressables.LoadSceneAsync(sceneKey, mode);
            await handle.Task;
        }
    }
}