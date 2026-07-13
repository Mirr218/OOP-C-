using System.Collections.Concurrent;
using System.Threading;
namespace task17;

public class ServerThread
{
    private readonly BlockingCollection<ICommand> _commands = new();
    private Thread? _thread;
    private bool _softStopRequested;
    private bool _hardStopRequested;
    internal bool IsCurrentThread => Thread.CurrentThread == _thread;

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
            var command = _commands.Take();
            command.Execute();

            if (_hardStopRequested)
            {
                break;
            }

            if (_softStopRequested && _commands.Count == 0)
            {
                break;
            }
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
