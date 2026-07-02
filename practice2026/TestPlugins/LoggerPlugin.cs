using PluginLib;

namespace TestPlugins;

[PluginLoad("Logger")]
public class LoggerPlugin : ICommand
{
    public void Execute()
    {
        ExecutionLog.Record("Logger");
    }
}
