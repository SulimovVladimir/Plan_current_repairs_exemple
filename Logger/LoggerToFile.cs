using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Plan_current_repairs.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Plan_current_repairs.Logger
{
    public class LoggerToFile :ILogger , IDisposable
    {
        string filePath;
        static object _lock = new object();
        string category;

        public LoggerToFile(string path, string _category)
        {
            filePath = path;
            category = _category;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled (LogLevel logLevel)
        {
            //return logLevel==LogLevel.Information;
            return true;
        }

        public void Log<Tstate>(LogLevel logLevel, EventId eventId, Tstate state, Exception? exception, Func<Tstate, Exception?,string> formatter)
        {
            if(formatter!=null)
            {
             lock (_lock)
                {
                    string fullFilePath=null;
                    string recordingLog=null;

                    if (exception!=null)
                    {
                        fullFilePath = Path.Combine(filePath, "logException.txt");
                        recordingLog = $"{logLevel}-----{category}-----{DateTime.Now}-----{exception.GetType().FullName}: {formatter(state, exception)} {Environment.NewLine}" +
                            $"{exception.StackTrace}{Environment.NewLine}";
                        EmailService emailService = new EmailService();
                        emailService.SendEmailAsync("email@domen.ru", "План-отчет по текущему ремонту", $"В системе зарегистрировано новое исключение: {recordingLog}"); 
                    }
                    else
                    {
                        fullFilePath = Path.Combine(filePath, "log.txt");
                        recordingLog = $"{logLevel}-----{category}-----{DateTime.Now}-----{formatter(state, exception)} {Environment.NewLine}";
                    }
                    File.AppendAllText(fullFilePath, recordingLog);
                }
            }
           
        }
        public void Dispose() { }
    }
}
