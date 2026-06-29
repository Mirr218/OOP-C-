using System;
using System.IO;
using System.Linq;
using System.Reflection;

if (args.Length == 0)
{
    Console.WriteLine("Использование: MetaInfoApp <путь_к_DLL>");
    Console.WriteLine("Пример: dotnet run --project MetaInfoApp -- /path/to/FileSystemCommands.dll");
    return;
}

string dllPath = args[0];

if (!File.Exists(dllPath))
{
    Console.WriteLine($"Ошибка: Файл {dllPath} не найден");
    return;
}

Assembly assembly;
try
{
    assembly = Assembly.LoadFrom(dllPath);
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка загрузки сборки: {ex.Message}");
    return;
}

Console.WriteLine($"Сборка: {assembly.GetName().Name}");
Console.WriteLine(new string('=', 60));

var types = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract);

foreach (Type type in types)
{
    Console.WriteLine($"\nКласс: {type.Name}");

    var classAttributes = type.GetCustomAttributes(false);
    if (classAttributes.Length > 0)
    {
        Console.WriteLine("  Атрибуты:");
        foreach (var attr in classAttributes)
        {
            Console.WriteLine($"    [{attr.GetType().Name}]");
        }
    }

    var constructors = type.GetConstructors();
    if (constructors.Length > 0)
    {
        Console.WriteLine("  Конструкторы:");
        foreach (var ctor in constructors)
        {
            var parameters = ctor.GetParameters();
            var paramList = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
            Console.WriteLine($"    .ctor({paramList})");
        }
    }

    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
    if (methods.Length > 0)
    {
        Console.WriteLine("  Методы:");
        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            var paramList = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
            Console.WriteLine($"    {method.ReturnType.Name} {method.Name}({paramList})");
        }
    }
}
