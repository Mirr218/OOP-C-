using Xunit;
using task04;
namespace task04tests;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(17, fighter.FirePower); // Пусть будет 17 урона у исстребителя
    }

    [Fact]
    public void Cruiser_ShouldBeStrongerThanFighter()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.FirePower < cruiser.FirePower);
    }

    [Fact]
    public void Cruiser_MoveForward_ShouldNotThrow()
    {
        ISpaceship cruiser = new Cruiser();
        cruiser.MoveForward();
    }

    [Fact]
    public void Cruiser_Rotate_ShouldNotThrow()
    {
        ISpaceship cruiser = new Cruiser();
        cruiser.Rotate(90);
    }

    [Fact]
    public void Cruiser_Fire_ShouldNotThrow()
    {
        ISpaceship cruiser = new Cruiser();
        cruiser.Fire();
    }

    [Fact]
    public void Fighter_MoveForward_ShouldNotThrow()
    {
        ISpaceship fighter = new Fighter();
        fighter.MoveForward();
    }

    [Fact]
    public void Fighter_Rotate_ShouldNotThrow()
    {
        ISpaceship fighter = new Fighter();
        fighter.Rotate(45);
    }

    [Fact]
    public void Fighter_Fire_ShouldNotThrow()
    {
        ISpaceship fighter = new Fighter();
        fighter.Fire();
    }
}