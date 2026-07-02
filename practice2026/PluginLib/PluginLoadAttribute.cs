using System;

namespace PluginLib;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class PluginLoadAttribute : Attribute
{
    public string Name { get; }
    public string[] Dependencies { get; }

    // Конструктор позволяет писать так:
    // [PluginLoad("Logger")]
    // [PluginLoad("Database", "Logger", "Config")]
    public PluginLoadAttribute(string name, params string[] dependencies)
    {
        Name = name;
        Dependencies = dependencies ?? Array.Empty<string>();
    }
}
