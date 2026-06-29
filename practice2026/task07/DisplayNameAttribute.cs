namespace task07;

public class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; }
    
    public DisplayNameAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}