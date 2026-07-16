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

    [Fact]
    public void ServerThread_LongRunningCommand_CompletesAfterSeveralSteps()
    {
        var serverThread = new ServerThread();
        var command = new StepCommand(steps: 3);

        serverThread.Start();
        serverThread.Enqueue(command);
        serverThread.Enqueue(new SoftStopCommand(serverThread));
        serverThread.Join();

        Assert.True(command.IsCompleted);
    }

    [Fact]
    public void ServerThread_TwoLongRunningCommands_ExecuteInRoundRobinOrder()
    {
        var log = new List<int>();
        var serverThread = new ServerThread();

        serverThread.Start();
        serverThread.Enqueue(new LoggingStepCommand(id: 1, steps: 3, log));
        serverThread.Enqueue(new LoggingStepCommand(id: 2, steps: 3, log));
        serverThread.Enqueue(new SoftStopCommand(serverThread));
        serverThread.Join();

        Assert.Equal([1, 2, 1, 2, 1, 2], log);
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

    private class StepCommand : ILongRunningCommand
    {
        private int _remainingSteps;

        public StepCommand(int steps)
        {
            _remainingSteps = steps;
        }

        public bool IsCompleted => _remainingSteps <= 0;

        public void Execute()
        {
            _remainingSteps--;
        }
    }

    private class LoggingStepCommand : ILongRunningCommand
    {
        private int _remainingSteps;
        private readonly int _id;
        private readonly List<int> _log;

        public LoggingStepCommand(int id, int steps, List<int> log)
        {
            _id = id;
            _remainingSteps = steps;
            _log = log;
        }

        public bool IsCompleted => _remainingSteps <= 0;

        public void Execute()
        {
            _log.Add(_id);
            _remainingSteps--;
        }
    }
}
