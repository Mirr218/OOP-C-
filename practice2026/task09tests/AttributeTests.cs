using System.Reflection;
using CommandLib;
using FileSystemCommands;
using Xunit;

namespace task09tests;

public class AttributeTests
{
    [Fact]
    public void DirectorySizeCommand_HasDisplayNameAttribute()
    {
        var type = typeof(DirectorySizeCommand);
        var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
        
        Assert.NotNull(attribute);
        Assert.Equal("Вычисление размера каталога", attribute.DisplayName);
    }

    [Fact]
    public void DirectorySizeCommand_HasVersionAttribute()
    {
        var type = typeof(DirectorySizeCommand);
        var attribute = type.GetCustomAttribute<VersionAttribute>();
        
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Major);
        Assert.Equal(0, attribute.Minor);
    }

    [Fact]
    public void FindFilesCommand_HasDisplayNameAttribute()
    {
        var type = typeof(FindFilesCommand);
        var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
        
        Assert.NotNull(attribute);
        Assert.Equal("Поиск файлов по маске", attribute.DisplayName);
    }

    [Fact]
    public void FindFilesCommand_HasVersionAttribute()
    {
        var type = typeof(FindFilesCommand);
        var attribute = type.GetCustomAttribute<VersionAttribute>();
        
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Major);
        Assert.Equal(0, attribute.Minor);
    }

    [Fact]
    public void DirectorySizeCommand_ExecuteMethod_HasDisplayNameAttribute()
    {
        var method = typeof(DirectorySizeCommand).GetMethod("Execute");
        Assert.NotNull(method);
        
        var attribute = method.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Выполнить команду", attribute.DisplayName);
    }

    [Fact]
    public void FindFilesCommand_ExecuteMethod_HasDisplayNameAttribute()
    {
        var method = typeof(FindFilesCommand).GetMethod("Execute");
        Assert.NotNull(method);
        
        var attribute = method.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Выполнить команду", attribute.DisplayName);
    }
}
