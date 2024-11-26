using Serilog;

namespace Infernos.Shared.Logs;

public static class Logger
{
    public static void LogExceptions(Exception ex)
    {

        LogToFile(ex.Message);
        LogToConsole(ex.Message);
        LogToDebugger(ex.Message);
    }

    public static void LogToFile(string message) => Log.Information(message);

    public static void LogToConsole(string message) => Log.Warning(message);
    public static void LogToDebugger(string message) => Log.Debug(message);
}
