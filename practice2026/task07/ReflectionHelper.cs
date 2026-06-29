using System;
using System.Reflection;

namespace task07;

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        var displayNameAttr = type.GetCustomAttribute<DisplayNameAttribute>();
        if (displayNameAttr != null)
        {
            Console.WriteLine($"Класс: {displayNameAttr.DisplayName}");
        }

        var versionAttr = type.GetCustomAttribute<VersionAttribute>();
        if (versionAttr != null)
        {
            Console.WriteLine($"Версия: {versionAttr.Major}.{versionAttr.Minor}");
        }

        Console.WriteLine("Методы:");
        foreach (var method in type.GetMethods())
        {
            var methodDisplayName = method.GetCustomAttribute<DisplayNameAttribute>();
            if (methodDisplayName != null)
            {
                Console.WriteLine($"  - {methodDisplayName.DisplayName}");
            }
        }

        Console.WriteLine("Свойства:");
        foreach (var property in type.GetProperties())
        {
            var propertyDisplayName = property.GetCustomAttribute<DisplayNameAttribute>();
            if (propertyDisplayName != null)
            {
                Console.WriteLine($"  - {propertyDisplayName.DisplayName}");
            }
        }
    }
}