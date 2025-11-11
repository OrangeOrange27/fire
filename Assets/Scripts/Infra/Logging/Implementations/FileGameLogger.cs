using System.IO;
using UnityEngine;

namespace Infra.Logging.Implementations
{

    public class FileGameLogger : IGameLogger
    {
        private readonly string _path;

        public FileGameLogger(string fileName = "game_log.txt")
        {
            _path = Path.Combine(Application.persistentDataPath, fileName);
        }

        public void Log(string message) => Write("INFO", message);
        public void LogWarning(string message) => Write("WARN", message);
        public void LogError(string message) => Write("ERROR", message);

        private void Write(string level, string message)
        {
            using (var sw = File.AppendText(_path))
                sw.WriteLine($"{System.DateTime.Now:HH:mm:ss} [{level}] {message}");
        }
    }
}