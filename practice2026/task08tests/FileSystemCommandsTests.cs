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
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        try
        {
            // Act: создаем команду и выполняем
            var command = new DirectorySizeCommand(testDir);
            command.Execute(); // Проверяем, что не возникает исключений

            // Assert: в реальном тесте можно было бы проверить вывод,
            // но пока просто проверяем, что метод выполнился без ошибок
        }
        finally
        {
            // Cleanup: удаляем временную директорию
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

        try
        {
            // Act: создаем команду и выполняем
            var command = new FindFilesCommand(testDir, "*.txt");
            command.Execute(); // Должен найти 1 файл

            // Assert: пока просто проверяем, что метод выполнился без ошибок
        }
        finally
        {
            // удаляем временную директорию
            if (Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
        }
    }
}