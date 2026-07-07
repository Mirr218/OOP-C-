namespace task07;


[AttributeUsage(AttributeTargets.Class)]
public class VersionAttribute : Attribute
{
    public int Major { get; }
    public int Minor { get; }
    
    public VersionAttribute(int major, int minor)
    {
        ArgumentException.ThrowIfNegative(major);
        ArgumentException.ThrowIfNegative(minor);

        Major = major;
        Minor = minor;
    }
}