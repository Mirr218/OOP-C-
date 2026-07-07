using task07;
using Xunit;

namespace task07tests;

public class ReflectionHelperTests
{
    [Fact]
    public void PrintTypeInfo_OutputsDisplayName()
    {
        // Arrange: перехватываем консольный вывод
        var stringWriter = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            // Act: вызываем метод
            ReflectionHelper.PrintTypeInfo(typeof(SampleClass));

            // Assert: проверяем вывод
            var output = stringWriter.ToString();
            Assert.Contains("Пример класса", output);
        }
        finally
        {
            // возвращаем консоль обратно
            Console.SetOut(originalOut);
            stringWriter.Dispose();
        }
    }

    [Fact]
    public void PrintTypeInfo_OutputsVersion()
    {
        var stringWriter = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            ReflectionHelper.PrintTypeInfo(typeof(SampleClass));

            var output = stringWriter.ToString();
            Assert.Contains("1.0", output);
        }
        finally
        {
            Console.SetOut(originalOut);
            stringWriter.Dispose();
        }
    }

    [Fact]
    public void PrintTypeInfo_OutputsMethodDisplayName()
    {
        var stringWriter = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            ReflectionHelper.PrintTypeInfo(typeof(SampleClass));

            var output = stringWriter.ToString();
            Assert.Contains("Тестовый метод", output);
        }
        finally
        {
            Console.SetOut(originalOut);
            stringWriter.Dispose();
        }
    }

    [Fact]
    public void PrintTypeInfo_OutputsPropertyDisplayName()
    {
        var stringWriter = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            ReflectionHelper.PrintTypeInfo(typeof(SampleClass));

            var output = stringWriter.ToString();
            Assert.Contains("Числовое свойство", output);
        }
        finally
        {
            Console.SetOut(originalOut);
            stringWriter.Dispose();
        }
    }
}