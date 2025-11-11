using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Infra.Logging.Implementations
{
    public class UnityGameLogger : IGameLogger
    {
        public void Log(string message) => LogInternal(message);
        public void LogWarning(string message) => LogWarningInternal(message);
        public void LogError(string message) => LogErrorInternal(message);

        [Conditional("UNITY_EDITOR")]
        private void LogInternal(string message) => Debug.Log(message);

        [Conditional("UNITY_EDITOR")]
        private void LogWarningInternal(string message) => Debug.Log(message);

        [Conditional("UNITY_EDITOR")]
        private void LogErrorInternal(string message) => Debug.Log(message);
    }
}