namespace task04;

public class Fighter : ISpaceship
{
    public int Speed => 100;
    public int FirePower => 17;
    public void MoveForward() => Console.WriteLine("Move Move Move!!! FASTER");
    public void Rotate(int angle) => Console.WriteLine($"Rotation on {angle} degrees");
    public void Fire() => Console.WriteLine("Pew, Pew, Pew");
}
