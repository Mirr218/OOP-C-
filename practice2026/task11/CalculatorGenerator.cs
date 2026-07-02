using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace task11;

public static class CalculatorGenerator
{
    public static T Generate<T>(string code, string typeName) where T : class
    {
        // TODO:
        // 1. Парсинг кода в синтаксическое дерево
        // 2. Компиляцию через Roslyn
        // 3. Загрузку сборки
        // 4. Создание экземпляра класса
        return null!;
    }
}
