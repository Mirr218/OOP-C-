using System;
using System.IO;
using FileSystemCommands;
using Xunit;

namespace task08tests;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        // Arrange: создаем временную директорию с файлами
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello"); // 5 байт
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World"); // 5 байт

        // Перехватываем консольный вывод
        var stringWriter = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            // Act: создаем команду и выполняем
            var command = new DirectorySizeCommand(testDir);
            command.Execute();

            // Assert: проверяем, что в выводе есть размер
            var output = stringWriter.ToString();
            Assert.Contains("10", output); // 5 + 5 = 10 байт
        }
        finally
        {
            Console.SetOut(originalOut);
            stringWriter.Dispose();
            if (Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
        }
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        // Arrange: создаем временную директорию с файлами
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        // Перехватываем консольный вывод
        var stringWriter = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            // Act: создаем команду и выполняем
            var command = new FindFilesCommand(testDir, "*.txt");
            command.Execute();

            // Assert: проверяем, что найден только .txt файл
            var output = stringWriter.ToString();
            Assert.Contains("file1.txt", output);
            Assert.DoesNotContain("file2.log", output);
        }
        finally
        {
            Console.SetOut(originalOut);
            stringWriter.Dispose();
            if (Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
        }
    }
}
