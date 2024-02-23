using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Plan_current_repairs.Logger;

namespace Plan_current_repairs.Extensions
{
    public static class LoggerToFileExtensions
    {
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder,string filePath)
        {
            builder.AddProvider(new LoggerToFileProvider(filePath));
            return builder;
        }
    }
}
