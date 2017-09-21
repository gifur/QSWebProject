using System;
using System.Diagnostics;
using System.Globalization;
using System.Security;

namespace QS.Common.Logging
{
    /// <summary>
    /// Implementation of contract 
    /// using System.Diagnostics API.
    /// </summary>
    public sealed class Net4Log : ILogger
    {
        public bool IsEnabled(LogLevel level)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level, Exception exception, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Debug(object item)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void LogInfo(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void LogError(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void LogError(string message, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
