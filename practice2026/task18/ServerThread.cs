using System.Collections.Concurrent;
using System.Threading;
namespace task18;

public class ServerThread
{
    private readonly BlockingCollection<ICommand> _commands = new();
    private Thread? _thread;
    private bool _softStopRequested;
    private bool _hardStopRequested;
    internal bool IsCurrentThread => Thread.CurrentThread == _thread;
    public Action<ICommand, Exception>? ExceptionHandler { get; set; }
    private readonly IScheduler _scheduler = new RoundRobinScheduler();
    
    public void Start()
    {
        _thread = new Thread(Run);
        _thread.Start();
    }

    public void Enqueue(ICommand command)
    {
        _commands.Add(command);
    }

    public void Join()
    {
        _thread?.Join();
    }

    internal void RequestSoftStop()
    {
        _softStopRequested = true;
    }

    internal void RequestHardStop()
    {
        _hardStopRequested = true;
    }

    private void Run()
    {
        while (true)
        {
            ICommand? command = null;

            if (_commands.TryTake(out var newCommand, 0))
            {
                command = newCommand;
            }
            else if (_scheduler.HasCommand())
            {
                command = _scheduler.Select();
            }
            else if (!_commands.TryTake(out newCommand, TimeSpan.FromMilliseconds(50)))
            {
                if (_hardStopRequested) break;
                if (_softStopRequested && _commands.Count == 0 && !_scheduler.HasCommand()) break;
                continue;
            }
            else
            {
                command = newCommand;
            }

            try
            {
                command.Execute();
            }
            catch (Exception exception)
            {
                ExceptionHandler?.Invoke(command, exception);
            }

            if (command is ILongRunningCommand longRunning && !longRunning.IsCompleted)
            {
                _scheduler.Add(command);
            }

            if (_hardStopRequested) break;
            if (_softStopRequested && _commands.Count == 0 && !_scheduler.HasCommand()) break;
        }
    }

    internal void ThrowIfNotCurrentThread()
    {
        if (!IsCurrentThread)
        {
            throw new InvalidOperationException("Stop command can be executed only inside its server thread.");
        }
    }
}
