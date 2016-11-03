using System;
using JetBrains.Annotations;

namespace GameCore.Core.Logging
{
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
}