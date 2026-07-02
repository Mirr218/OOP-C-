using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PluginLib;

namespace PluginManager;

public class PluginManager
{
    public void LoadPlugins(string directoryPath)
    {
        // 1. Проверяем существование директории
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"Директория не найдена: {directoryPath}");
        }

        // 2. Находим все DLL в папке
        var dllFiles = Directory.GetFiles(directoryPath, "*.dll");

        if (dllFiles.Length == 0)
        {
            return; // Пустая папка — ничего не делаем
        }

        // 3. Загружаем все DLL и находим плагины
        var plugins = new List<(Type Type, PluginLoadAttribute Attribute)>();

        foreach (var dllFile in dllFiles)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllFile);
                
                foreach (var type in assembly.GetTypes())
                {
                    // Ищем классы с атрибутом PluginLoad
                    var attribute = type.GetCustomAttribute<PluginLoadAttribute>();
                    if (attribute != null && type.IsClass && !type.IsAbstract)
                    {
                        plugins.Add((type, attribute));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки {dllFile}: {ex.Message}");
            }
        }

        if (plugins.Count == 0)
        {
            return;
        }

        var sortedPlugins = TopologicalSort(plugins);

        foreach (var type in sortedPlugins)
        {
            var instance = Activator.CreateInstance(type) as ICommand; // Динамически создаем экземпляр класса по типу (можем это сделать, потому что конструктор команды по условию не со параматров)
            instance?.Execute();
        }
    }

    private List<Type> TopologicalSort(List<(Type Type, PluginLoadAttribute Attribute)> plugins)
    {
        // Маппинг: имя плагина → тип
        var nameToType = new Dictionary<string, Type>();
        foreach (var (type, attr) in plugins)
        {
            nameToType[attr.Name] = type;
        }

        // Подсчитываем входящие рёбра (количество зависимостей)
        var inDegree = new Dictionary<Type, int>();
        var dependents = new Dictionary<Type, List<Type>>();

        foreach (var (type, attr) in plugins)
        {
            inDegree[type] = attr.Dependencies.Length;
            dependents[type] = new List<Type>();
        }

        // Строим обратные связи: кто зависит от кого
        foreach (var (type, attr) in plugins)
        {
            foreach (var dependencyName in attr.Dependencies)
            {
                if (nameToType.TryGetValue(dependencyName, out var dependencyType))
                {
                    dependents[dependencyType].Add(type);
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Плагин '{attr.Name}' зависит от несуществующего плагина '{dependencyName}'");
                }
            }
        }

        // Алгоритм Кана: находим вершины с входящими = 0
        var queue = new Queue<Type>();
        foreach (var (type, degree) in inDegree)
        {
            if (degree == 0)
            {
                queue.Enqueue(type);
            }
        }

        var result = new List<Type>();

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);

            // "Удаляем" текущую вершину: уменьшаем счётчики у зависимых
            foreach (var dependent in dependents[current])
            {
                inDegree[dependent]--;
                if (inDegree[dependent] == 0)
                {
                    queue.Enqueue(dependent);
                }
            }
        }

        // Проверка на цикл
        if (result.Count != plugins.Count)
        {
            throw new InvalidOperationException("Обнаружен цикл в зависимостях плагинов");
        }

        return result;
    }
}
