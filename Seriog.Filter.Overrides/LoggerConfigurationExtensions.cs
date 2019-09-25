namespace Serilog.Filter.Overrides
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerFilterConfiguration Filter(this LoggerConfiguration @this)
        {
            return new LoggerFilterConfiguration(@this);
        }
    }
}
