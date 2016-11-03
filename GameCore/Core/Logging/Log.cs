using System;
using JetBrains.Annotations;
using Debug = UnityEngine.Debug;

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
        }
    }

    public interface ILogAppender
    {
        [StringFormatMethod("formatedString")]
        void Info(string formatedString, params object[] parameters);

        [StringFormatMethod("formatedString")]
        void Warning(string formatedString, params object[] parameters);

        [StringFormatMethod("formatedString")]
        void Error(string formatedString, params object[] parameters);

        void Exception(Exception exception);
    }

    public class MultiplyLogAppender : ILogAppender
    {
        private readonly ILogAppender[] _logAppender;

        public MultiplyLogAppender(params ILogAppender[] logAppender)
        {
            _logAppender = logAppender;
        }

        public void Info(string formatedString, params object[] parameters)
        {
            foreach (var appender in _logAppender)
            {
                appender.Info(formatedString,parameters);
            }
        }

        public void Warning(string formatedString, params object[] parameters)
        {
            foreach (var appender in _logAppender)
            {
                appender.Warning(formatedString, parameters);
            }
        }

        public void Error(string formatedString, params object[] parameters)
        {
            foreach (var appender in _logAppender)
            {
                appender.Error(formatedString, parameters);
            }
        }

        public void Exception(Exception exception)
        {
            foreach (var appender in _logAppender)
            {
                appender.Exception(exception);
            }
        }
    }

    public class DefaultLogAppender : ILogAppender
    {
        private readonly string _contextName;
        
        public void Info(string formatedString, params object[] parameters)
        {
            Debug.Log(string.Format(formatedString, parameters));
        }

        public void Warning(string formatedString, params object[] parameters)
        {
            Debug.LogWarningFormat(formatedString, parameters);
        }

        public void Error(string formatedString, params object[] parameters)
        {
            Debug.LogErrorFormat(formatedString, parameters);
        }

        public void Exception(Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}
