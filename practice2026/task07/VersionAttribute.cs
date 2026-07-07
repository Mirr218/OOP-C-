namespace task07;


[AttributeUsage(AttributeTargets.Class)]
public class VersionAttribute : Attribute
{
    public int Major { get; }
    public int Minor { get; }
    
    public VersionAttribute(int major, int minor)
    {
        if (major < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(major), "Major version cannot be negative.");
        }
        
        if (minor < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minor), "Minor version cannot be negative.");
        }

        Major = major;
        Minor = minor;
    }
}