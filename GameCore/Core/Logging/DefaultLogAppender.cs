using System;
using UnityEngine;

namespace GameCore.Core.Logging
{
    public class DefaultLogAppender : ILogAppender
    {
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