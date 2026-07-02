using PluginLib;

namespace TestPlugins;

[PluginLoad("Database", "Logger")]
public class DatabasePlugin : ICommand
{
    public void Execute()
    {
        ExecutionLog.Record("Database");
    }
}
