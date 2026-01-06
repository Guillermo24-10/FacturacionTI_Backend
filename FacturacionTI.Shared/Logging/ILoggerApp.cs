namespace FacturacionTI.Shared.Logging
{
    public interface ILoggerApp
    {
        void Info(string message, object? data = null);
        void Warning(string message, object? data = null);
        void Error(string message, Exception? ex = null, object? data = null);
        void Fatal(string message, Exception? ex = null);
    }
}
