using Serilog.Core;
using Serilog.Events;
using System.Collections.Concurrent;

namespace Seriog.Filter.Overrides.Tests
{
    class TestSink : ILogEventSink
    {
        public ConcurrentBag<LogEvent> Events { get; } = new ConcurrentBag<LogEvent>();
        public void Emit(LogEvent logEvent)
        {
            Events.Add(logEvent);
        }
    }
}
