using Cysharp.Threading.Tasks;

namespace Server
{
    public static class ServerAPI
    {
        private const string BaseUrl = "http://localhost:8080/api";

        public static async UniTask<string> LoginAsync()
        {
            var url = $"{BaseUrl}/auth/login";

            var token = TokenManager.GetToken();

            var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(token);

            var response = await ServerRequest.PostRequest<string>(url, jsonBody);

            return response;
        }
        
        public static async UniTask<string> SendEmpty()
        {
            var url = $"{BaseUrl}/click";
            var response = await ServerRequest.GetRequest<string>(url);
            return response;
        }
    }
}