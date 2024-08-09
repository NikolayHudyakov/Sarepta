using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace ProductLabeling.Logging
{
    internal static class LoggerExtensions
    {
        public static void Trace(this ILogger logger, string? message = null, [CallerMemberName] string? memberName = null) =>
            logger.LogTrace($"[{memberName}] {message}");

        public static void Debug(this ILogger logger, string? message = null, [CallerMemberName] string? memberName = null) =>
            logger.LogDebug($"[{memberName}] {message}");

        public static void Info(this ILogger logger, string? message = null, [CallerMemberName] string? memberName = null) =>
            logger.LogInformation($"[{memberName}] {message}");

        public static void Warn(this ILogger logger, string? message = null, [CallerMemberName] string? memberName = null) =>
            logger.LogWarning($"[{memberName}] {message}");

        public static void Error(this ILogger logger, string? message = null, [CallerMemberName] string? memberName = null) =>
            logger.LogError($"[{memberName}] {message}");

        public static void Fatal(this ILogger logger, string? message = null, [CallerMemberName] string? memberName = null) =>
            logger.LogCritical($"[{memberName}] {message}");
    }
}
