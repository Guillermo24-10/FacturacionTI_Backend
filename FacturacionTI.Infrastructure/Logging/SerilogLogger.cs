using Serilog;

namespace FacturacionTI.Shared.Logging
{
    public class SerilogLogger : ILoggerApp
    {
        public SerilogLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(
                    "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    shared: true)
                .CreateLogger();
        }
        public void Info(string mensaje, object? data = null)
            => Log.Information("{Mensaje} {@Data}", mensaje, data);

        public void Warning(string mensaje, object? data = null)
            => Log.Warning("{Mensaje} {@Data}", mensaje, data);

        public void Error(string mensaje, Exception? ex = null, object? data = null)
            => Log.Error(ex, "{Mensaje} {@Data}", mensaje, data);

        public void Fatal(string mensaje, Exception? ex = null)
            => Log.Fatal(ex, "{Mensaje}", mensaje);
    }
}
