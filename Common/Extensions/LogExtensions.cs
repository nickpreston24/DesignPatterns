using NLog;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Common
{
    public static partial class CommonExtensions
    {
        private static readonly Logger NLogger = LogManager.GetCurrentClassLogger();

        public static void NLog(this string message) => NLogger.Debug(message);

        public static void NLog(this Exception exception, string message = null) => NLogger.Error(exception, message);

        public static bool Log(this Exception ex, [CallerMemberName] string message = null, params object[] args)
        {
            string logMessage = $"Exception: {message}";
            Debug.Print(ex.ToString(), args);
            NLog(ex, logMessage);
            return false;
        }
    }
}
