using Infra.Logging.Implementations;

namespace Infra.Logging
{
    using System.Diagnostics;

    public static class LoggingFacade
    {
        static LoggingFacade()
        {
#if UNITY_EDITOR
            Instance = new UnityGameLogger();
#else
        Instance = new FileGameLogger();
#endif
        }

        private static IGameLogger Instance { get; }

        public static void Log(string message) => Instance?.Log(message);
        public static void LogWarning(string message) => Instance?.LogWarning(message);
        public static void LogError(string message) => Instance?.LogError(message);
    }
}