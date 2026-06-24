namespace task04;

public class Cruiser : ISpaceship
{
    public int Speed => 50;
    public int FirePower => 100;
    public void MoveForward() => Console.WriteLine("Move Move Move!!!");
    public void Rotate(int angle) => Console.WriteLine($"Rotation on {angle} degrees");
    public void Fire() => Console.WriteLine("KABOOOOM");
}
