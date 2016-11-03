using System;

namespace GameCore.Core.Logging
{
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
}