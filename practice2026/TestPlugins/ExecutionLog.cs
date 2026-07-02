using System.Collections.Generic;

namespace TestPlugins;

/// Статический журнал, куда плагины записывают факт своего выполнения.
/// Используется в тестах для проверки порядка загрузки плагинов.
public static class ExecutionLog
{
    private static readonly List<string> _log = new();

    // Добавить запись о выполнении плагина.
    public static void Record(string pluginName)
    {
        _log.Add(pluginName);
    }

    // Получить список выполненных плагинов в порядке выполнения.
    public static IReadOnlyList<string> GetLog() => _log.AsReadOnly();

    // Очистить журнал (вызывается перед каждым тестом).
    public static void Clear() => _log.Clear();
}
