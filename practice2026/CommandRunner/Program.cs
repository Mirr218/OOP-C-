using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLib;

// 1. Вычисляем путь к DLL-файлу с командами
string baseDir = AppContext.BaseDirectory;
string dllPath = Path.GetFullPath(Path.Combine(
    baseDir, "..", "..", "..", "..", 
    "FileSystemCommands", "bin", "Debug", "net10.0", "FileSystemCommands.dll"));

Console.WriteLine($"Ищем библиотеку по пути: {dllPath}");

if (!File.Exists(dllPath))
{
    Console.WriteLine("Ошибка: Библиотека FileSystemCommands.dll не найдена!");
    Console.WriteLine("Убедитесь, что вы выполнили 'dotnet build' для всего решения.");
    return;
}

// 2. Динамически загружаем сборку в память
Assembly assembly = Assembly.LoadFrom(dllPath);
Console.WriteLine($"Сборка '{assembly.GetName().Name}' успешно загружена!\n");

// 3. Ищем все классы, которые реализуют интерфейс ICommand
var commandTypes = assembly.GetTypes()
    .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

Console.WriteLine($"Найдено команд: {commandTypes.Count()}");
Console.WriteLine(new string('-', 40));

// 4. Создаем экземпляры и выполняем их
foreach (Type type in commandTypes)
{
    Console.WriteLine($"Выполняем команду: {type.Name}");
    
    // Получаем первый публичный конструктор
    var constructor = type.GetConstructors().FirstOrDefault();
    
    if (constructor != null)
    {
        // Узнаем, какие параметры нужны конструктору
        var parameters = constructor.GetParameters();
        object[] ctorArgs = new object[parameters.Length];

        // Хитрость: заполняем параметры "умными" заглушками
        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType == typeof(string))
            {
                // Первый строковый параметр считаем путем, второй - маской
                ctorArgs[i] = i == 0 ? Directory.GetCurrentDirectory() : "*.cs"; 
            }
        }

        // Создаем экземпляр класса динамически!
        var command = (ICommand)Activator.CreateInstance(type, ctorArgs)!;
        
        // Выполняем команду
        command.Execute();
    }
    
    Console.WriteLine(new string('-', 40));
}