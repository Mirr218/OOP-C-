using System.Text;

namespace task19;

public static class Task19Illustration
{
    public static IReadOnlyList<string> Run(TextWriter? mirrorOutput = null)
    {
        var lines = new List<string>();
        ServerThread? serverThread = null;
        var stopEnqueued = 0;

        var capture = new LineCapturingWriter(lines, mirrorOutput, () =>
        {
            if (lines.Count == 15 && Interlocked.CompareExchange(ref stopEnqueued, 1, 0) == 0)
            {
                serverThread!.Enqueue(new HardStopCommand(serverThread));
            }
        });

        var previousOutput = Console.Out;
        Console.SetOut(capture);

        try
        {
            serverThread = new ServerThread();
            serverThread.Start();

            for (var id = 1; id <= 5; id++)
            {
                serverThread.Enqueue(new TestCommand(id));
            }

            serverThread.Join();
        }
        finally
        {
            Console.SetOut(previousOutput);
        }

        return lines;
    }

    private sealed class LineCapturingWriter : TextWriter
    {
        private readonly List<string> _lines;
        private readonly TextWriter? _mirrorOutput;
        private readonly Action _onLineAdded;

        public LineCapturingWriter(List<string> lines, TextWriter? mirrorOutput, Action onLineAdded)
        {
            _lines = lines;
            _mirrorOutput = mirrorOutput;
            _onLineAdded = onLineAdded;
        }

        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string? value)
        {
            if (value is null)
            {
                return;
            }

            lock (_lines)
            {
                _lines.Add(value);
            }

            _mirrorOutput?.WriteLine(value);
            _onLineAdded();
        }
    }
}
