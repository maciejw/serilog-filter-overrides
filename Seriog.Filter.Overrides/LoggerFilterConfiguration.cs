using Serilog;
using Serilog.Events;

namespace Serilog.Filter.Overrides
{
    public class LoggerFilterConfiguration
    {
        private readonly LoggerConfiguration configuration;

        public LoggerFilterConfiguration(LoggerConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public LoggerConfiguration Overrides(LoggerSourceContextLevelOverrides switches)
        {
            configuration.Filter.ByExcluding(e => EventsBelowCertainLevel(e, switches));
            return configuration;
        }

        public static bool EventsBelowCertainLevel(LogEvent logEvent, LoggerSourceContextLevelOverrides globalSwitches)
        {
            var (defaultLevel, matchers) = globalSwitches.Current;

            for (int i = 0; i < matchers.Length; i++)
            {
                var filter = matchers[i];
                if (filter.Key(logEvent))
                {
                    return logEvent.Level < filter.Value;
                }
            }

            return logEvent.Level < defaultLevel;
        }
    }
}
