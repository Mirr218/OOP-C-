namespace task19;

public class TestCommand(int id) : ICommand
{
    int counter = 0;

    public void Execute()
    {
        Console.WriteLine($"Поток {id} вызов {++counter}");
    }
}
