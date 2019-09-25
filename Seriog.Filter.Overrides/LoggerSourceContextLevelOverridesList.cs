using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Events;
using Serilog.Filters;

namespace Serilog.Filter.Overrides
{
    public class LoggerSourceContextLevelOverridesList : SortedList<string, LogEventLevel>
    {
        private readonly static Comparer<string> descendingComparer = Comparer<string>.Create((x, y) => StringComparer.InvariantCultureIgnoreCase.Compare(y, x));
        public LoggerSourceContextLevelOverridesList(params KeyValuePair<string, LogEventLevel>[] sourceContextFilters) : base(sourceContextFilters.ToDictionary(s => s.Key, s => s.Value), descendingComparer) { }
        public KeyValuePair<Func<LogEvent, bool>, LogEventLevel>[] GetMatchers()
        {
            return this.Select(p => new KeyValuePair<Func<LogEvent, bool>, LogEventLevel>(Matching.FromSource(p.Key), p.Value)).ToArray();
        }
    }
}
