using System;
using System.IO;
using CommandLib;

namespace FileSystemCommands;

public class FindFilesCommand : ICommand
{
    private readonly string _path;
    private readonly string _mask;
    
    public FindFilesCommand(string path, string mask)
    {
        _path = path;
        _mask = mask;
    }
    
    public void Execute()
    {
        if (!Directory.Exists(_path))
        {
            Console.WriteLine($"Каталог {_path} не существует");
            return;
        }

        var files = Directory.GetFiles(_path, _mask, SearchOption.AllDirectories);
        
        Console.WriteLine($"Найдено файлов: {files.Length}");
        foreach (var file in files)
        {
            Console.WriteLine($"  {Path.GetFileName(file)}");
        }
    }
}
