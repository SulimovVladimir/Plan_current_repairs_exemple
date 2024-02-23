using Microsoft.Extensions.Logging;

namespace Plan_current_repairs.Logger
{
    public class LoggerToFileProvider : ILoggerProvider
    {
        string path;
        public LoggerToFileProvider(string path)
        {
            this.path = path;
        }
        public ILogger CreateLogger(string categoryName)
        {
           return new LoggerToFile(path, categoryName);
        }

        public void Dispose() { }
    }
}
