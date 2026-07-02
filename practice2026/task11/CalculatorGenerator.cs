using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace task11;

public static class CalculatorGenerator
{
    public static T Generate<T>(string code, string typeName) where T : class
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);


        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .Cast<MetadataReference>()
            .ToList();

        references.Add(MetadataReference.CreateFromFile(typeof(T).Assembly.Location));

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName: "DynamicAssembly",
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        // 4. Компилируем в MemoryStream
        using var ms = new MemoryStream();
        EmitResult result = compilation.Emit(ms);

        if (!result.Success)
        {
            var errors = result.Diagnostics
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(d => d.GetMessage());
            
            throw new InvalidOperationException(
                $"Ошибка компиляции:\n{string.Join("\n", errors)}");
        }

        ms.Seek(0, SeekOrigin.Begin);
        Assembly assembly = Assembly.Load(ms.ToArray());

        Type? type = assembly.GetType(typeName);
        if (type == null)
        {
            throw new TypeLoadException($"Тип '{typeName}' не найден в скомпилированной сборке");
        }

        object? instance = Activator.CreateInstance(type);
        if (instance == null)
        {
            throw new InvalidOperationException($"Не удалось создать экземпляр типа '{typeName}'");
        }

        return (T)instance;
    }
}
