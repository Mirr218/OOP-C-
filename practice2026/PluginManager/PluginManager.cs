using System;
using System.IO;

namespace PluginManager;

public class PluginManager
{
    public void LoadPlugins(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"Директория не найдена: {directoryPath}");
        }

        // TODO:
        // 1. Поиск всех DLL в папке
        // 2. Загрузку каждой DLL
        // 3. Поиск классов с атрибутом PluginLoad
        // 4. Построение графа зависимостей
        // 5. Топологическую сортировку
        // 6. Создание экземпляров и вызов Execute()
    }
}
