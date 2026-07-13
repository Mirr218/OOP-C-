using task17;
namespace task17tests;

public class ServerThreadTests
{
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

    [Fact]
    public void SoftStop_ExecutesCommandsRemainingInQueue()
    {
        var log = new List<string>();
        var serverThread = new ServerThread();

        serverThread.Start();
        
        serverThread.Enqueue(new ActionCommand(() => log.Add("Command 1 executed")));
        serverThread.Enqueue(new SoftStopCommand(serverThread));
        serverThread.Enqueue(new ActionCommand(() => log.Add("Command 2 executed")));
        
        serverThread.Join();

        Assert.Equal(["Command 1 executed", "Command 2 executed"], log);
    }
    [Fact]
    public void HardStop_DoesNotExecuteCommandsRemainingInQueue() 
    {
        var log = new List<string>();
        var serverThread = new ServerThread();

        serverThread.Start();

        serverThread.Enqueue(new ActionCommand(() => log.Add("Command 1 executed")));
        serverThread.Enqueue(new HardStopCommand(serverThread));
        serverThread.Enqueue(new ActionCommand(() => log.Add("Command 2 executed")));

        serverThread.Join();

        Assert.Equal(["Command 1 executed"], log);
    }

    [Fact]
    public void SoftStop_WhenExecutedOutsideServerThread_ThrowsException()
    {
        var serverThread = new ServerThread();
        var command = new SoftStopCommand(serverThread);
        Assert.Throws<InvalidOperationException>(() => command.Execute());
    }

    [Fact]
    public void HardStop_WhenExecutedOutsideServerThread_ThrowsException()
    {
        var serverThread = new ServerThread();
        var command = new HardStopCommand(serverThread);
        Assert.Throws<InvalidOperationException>(() => command.Execute());
    }
}
