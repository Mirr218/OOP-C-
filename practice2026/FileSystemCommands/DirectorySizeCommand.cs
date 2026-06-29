using System;
using System.IO;
using System.Linq;
using CommandLib;

namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    private readonly string _path;
    
    public DirectorySizeCommand(string path)
    {
        _path = path;
    }
    
    public void Execute()
    {
        if (!Directory.Exists(_path))
        {
            Console.WriteLine($"Каталог {_path} не существует");
            return;
        }

        var files = Directory.GetFiles(_path, "*", SearchOption.AllDirectories);
        
        long totalSize = files.Sum(file => new FileInfo(file).Length);
        
        Console.WriteLine($"Размер каталога {_path}: {totalSize} байт");
    }
}
