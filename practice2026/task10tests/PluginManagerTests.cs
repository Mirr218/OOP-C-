using System;
using System.IO;
using System.Reflection;
using PluginManager;
using TestPlugins;
using Xunit;

namespace task10tests;

public class PluginManagerTests : IDisposable
{
    private readonly string _testPluginsDir;

    public PluginManagerTests()
    {
        // Создаём временную папку для тестов
        _testPluginsDir = Path.Combine(Path.GetTempPath(), $"TestPlugins_{Guid.NewGuid()}");
        Directory.CreateDirectory(_testPluginsDir);

        // Копируем TestPlugins.dll в тестовую папку
        var testPluginsDll = GetTestPluginsDllPath();
        if (File.Exists(testPluginsDll))
        {
            File.Copy(testPluginsDll, Path.Combine(_testPluginsDir, "TestPlugins.dll"));
        }

        // Очищаем журнал выполнения перед каждым тестом
        ExecutionLog.Clear();
    }

    public void Dispose()
    {
        // Удаляем временную папку после теста
        if (Directory.Exists(_testPluginsDir))
        {
            Directory.Delete(_testPluginsDir, true);
        }
    }

    private string GetTestPluginsDllPath()
    {
        // Получаем путь к TestPlugins.dll относительно текущего теста
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var assemblyDir = Path.GetDirectoryName(assemblyLocation)!;
        return Path.Combine(assemblyDir, "TestPlugins.dll");
    }

    [Fact]
    public void LoadPlugins_ShouldLoadAllPlugins()
    {
        // Arrange
        var manager = new PluginManager.PluginManager();

        // Act
        manager.LoadPlugins(_testPluginsDir);

        // Assert: все 3 плагина должны выполниться
        var log = ExecutionLog.GetLog();
        Assert.Equal(3, log.Count);
    }

    [Fact]
    public void LoadPlugins_ShouldLoadInCorrectOrder()
    {
        // Arrange
        var manager = new PluginManager.PluginManager();

        // Act
        manager.LoadPlugins(_testPluginsDir);

        // Assert: порядок должен быть Logger → Database → App
        var log = ExecutionLog.GetLog();
        
        Assert.Equal(3, log.Count);
        Assert.Equal("Logger", log[0]);
        Assert.Equal("Database", log[1]);
        Assert.Equal("App", log[2]);
    }

    [Fact]
    public void LoadPlugins_ShouldHandleEmptyDirectory()
    {
        // Arrange
        var emptyDir = Path.Combine(_testPluginsDir, "Empty");
        Directory.CreateDirectory(emptyDir);
        var manager = new PluginManager.PluginManager();

        // Act
        manager.LoadPlugins(emptyDir);

        // Assert: ничего не должно выполниться
        var log = ExecutionLog.GetLog();
        Assert.Empty(log);
    }

    [Fact]
    public void LoadPlugins_ShouldThrowIfDirectoryNotExists()
    {
        // Arrange
        var nonExistentDir = Path.Combine(_testPluginsDir, "NonExistent");
        var manager = new PluginManager.PluginManager();

        // Act & Assert
        Assert.Throws<DirectoryNotFoundException>(() => manager.LoadPlugins(nonExistentDir));
    }
}
