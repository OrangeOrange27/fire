using System.Text;
using Cysharp.Threading.Tasks;
using Infra.Logging;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace Server
{
    public static class ServerRequest
    {
        private const string TokenHeader = "UserToken";

        public static async UniTask<T> PostRequest<T>(string url, string jsonBody)
        {
            using var request = new UnityWebRequest(url, "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                LoggingFacade.LogError($"POST request failed: {request.error}");
                return default;
            }

            var responseText = request.downloadHandler.text;

            if (typeof(T) == typeof(string))
            {
                return (T)(object)responseText;
            }

            return JsonConvert.DeserializeObject<T>(responseText);
        }

        public static async UniTask<T> GetRequest<T>(string url)
        {
            using var request = UnityWebRequest.Get(url);

            request.SetRequestHeader(TokenHeader, TokenManager.GetToken());

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                LoggingFacade.LogError($"GET request failed: {request.error}");
                return default;
            }

            var responseText = request.downloadHandler.text;

            if (typeof(T) == typeof(string))
            {
                return (T)(object)responseText;
            }

            return JsonConvert.DeserializeObject<T>(responseText);
        }
    }
}