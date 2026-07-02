using PluginLib;

namespace TestPlugins;

[PluginLoad("App", "Database", "Logger")]
public class AppPlugin : ICommand
{
    public void Execute()
    {
        ExecutionLog.Record("App");
    }
}