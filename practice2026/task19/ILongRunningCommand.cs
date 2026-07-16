namespace task19;

public interface ILongRunningCommand : ICommand
{
    bool IsCompleted { get; }
}
