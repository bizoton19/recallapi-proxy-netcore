using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Opendata.Recalls.Commands;

namespace Opendata.Recalls.Common
{
    public class Logger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }
    }

}