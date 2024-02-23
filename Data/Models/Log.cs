using System;

namespace Plan_current_repairs.Data.Models
{
    public class Log
    {
        public string LogLevel { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}
