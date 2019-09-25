using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using Xunit;
using Serilog.Filter.Overrides;
using Serilog;
using Xunit.Abstractions;
using System.Diagnostics;

namespace Seriog.Filter.Overrides.Tests
{

    public class FilterOverridesTests
    {
        private readonly ITestOutputHelper testOutput;

        public FilterOverridesTests(ITestOutputHelper testOutput)
        {
            this.testOutput = testOutput;
        }
        [Fact(DisplayName = "Should filter event with filter overrides")]
        public void Fact3()
        {
            var testSink = new TestSink();
            var defaultSink = new TestSink();

            var switches = new LoggerSourceContextLevelOverrides(LogEventLevel.Warning,
                KeyValuePair.Create("A.B", LogEventLevel.Debug)
            );

            var testLogger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Logger(c => c
                    .Filter().Overrides(switches)
                    .WriteTo.Sink(testSink))
                .CreateLogger();

            var defaultLogger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .MinimumLevel.Override("A.B", LogEventLevel.Debug)
                .WriteTo.Sink(defaultSink)
                .CreateLogger();

            var abLogger = testLogger.ForContext(Constants.SourceContextPropertyName, "A.B");
            var bLogger = testLogger.ForContext(Constants.SourceContextPropertyName, "B");


            Measure("Overrides, excluded by default level", () =>
            {
                for (int i = 0; i < 100_000; i++)
                {
                    bLogger.Information("b");
                }
            });
            Assert.Empty(testSink.Events);

            testSink.Events.Clear();

            Measure("Overrides, included by override category level", () =>
            {
                for (int i = 0; i < 100_000; i++)
                {
                    abLogger.Debug("ab");
                }
            });
            Assert.Equal(100000, testSink.Events.Count);
            testSink.Events.Clear();


            var abDefalutLogger = defaultLogger.ForContext(Constants.SourceContextPropertyName, "A.B");
            var bDefaultLogger = defaultLogger.ForContext(Constants.SourceContextPropertyName, "B");


            Measure("MinimumLevel, excluded by default level", () =>
            {
                for (int i = 0; i < 100_000; i++)
                {
                    bDefaultLogger.Information("b");
                }
            });
            Assert.Empty(defaultSink.Events);

            defaultSink.Events.Clear();
            Measure("MinimumLevel, included by override category level", () =>
            {
                for (int i = 0; i < 100_000; i++)
                {
                    abDefalutLogger.Debug("ab");
                }
            });
            Assert.Equal(100000, defaultSink.Events.Count);
            defaultSink.Events.Clear();

        }

        void Measure(string category, Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            testOutput.WriteLine($"Category: {category} elapsed {stopwatch.ElapsedMilliseconds} ms");

        }
    }
}
