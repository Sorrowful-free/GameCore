using System;
using JetBrains.Annotations;

namespace GameCore.Core.Logging
{
    public static class Log
    {
        private static ILogAppender _logAppender;
        
        static Log()
        {
            _logAppender = new DefaultLogAppender();
        }

        public static bool EnableLogs = true;
        public static void SetLogAppender(ILogAppender logAppender)
        {
            _logAppender = logAppender;
        }

        [StringFormatMethod("formatedString")]
        public static void Info(string formatedString, params object[] parameters)
        {
            if(EnableLogs)
                _logAppender.Info(formatedString,parameters);
        }

        [StringFormatMethod("formatedString")]
        public static void Warning(string formatedString, params object[] parameters)
        {
            if (EnableLogs)
                _logAppender.Warning(formatedString, parameters);
        }

        [StringFormatMethod("formatedString")]
        public static void Error(string formatedString, params object[] parameters)
        {
            if (EnableLogs)
                _logAppender.Error(formatedString, parameters);
        }

        public static void Exception(Exception exception)
        {
            if (EnableLogs)
                _logAppender.Exception(exception);
            else
                throw exception;
        }
    }
}
