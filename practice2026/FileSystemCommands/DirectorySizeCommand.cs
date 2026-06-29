using System;
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
        // Заглушка: пока ничего не делает
    }
}