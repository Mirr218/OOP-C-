using task19;

namespace task19tests;

public class Task19IllustrationTests
{
    [Fact]
    public void Illustration_RunsFiveTestCommandsThreeTimesEach_InRoundRobinOrder()
    {
        var lines = Task19Illustration.Run();

        Assert.Equal(15, lines.Count);

        for (var round = 1; round <= 3; round++)
        {
            for (var id = 1; id <= 5; id++)
            {
                var index = (round - 1) * 5 + (id - 1);
                Assert.Equal($"Поток {id} вызов {round}", lines[index]);
            }
        }
    }

    [Fact]
    public void Illustration_HardStop_PreventsFurtherExecution()
    {
        var lines = Task19Illustration.Run();

        Assert.Equal(15, lines.Count);

        for (var id = 1; id <= 5; id++)
        {
            var commandLines = lines.Where(line => line.StartsWith($"Поток {id} ", StringComparison.Ordinal)).ToList();
            Assert.Equal(3, commandLines.Count);
            Assert.Equal(
                [$"Поток {id} вызов 1", $"Поток {id} вызов 2", $"Поток {id} вызов 3"],
                commandLines);
        }
    }

    [Fact]
    public void ServerThread_LongRunningCommand_StillCompletesAfterSeveralSteps()
    {
        var serverThread = new ServerThread();
        var command = new StepCommand(steps: 3);

        serverThread.Start();
        serverThread.Enqueue(command);
        serverThread.Enqueue(new SoftStopCommand(serverThread));
        serverThread.Join();

        Assert.True(command.IsCompleted);
    }

    private sealed class StepCommand : ILongRunningCommand
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
}
