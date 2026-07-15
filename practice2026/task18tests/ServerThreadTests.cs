using task18;

namespace task18tests;

public class ServerThreadTests
{
    [Fact]
    public void ServerThread_ExecutesCommand()
    {
        var log = new List<string>();
        var serverThread = new ServerThread();

        serverThread.Start();
        serverThread.Enqueue(new ActionCommand(() => log.Add("Command executed")));
        serverThread.Enqueue(new SoftStopCommand(serverThread));
        serverThread.Join();

        Assert.Single(log);
        Assert.Equal("Command executed", log[0]);
    }

    private class ActionCommand : ICommand
    {
        private readonly Action _action;

        public ActionCommand(Action action)
        {
            _action = action;
        }

        public void Execute()
        {
            _action();
        }
    }
}
