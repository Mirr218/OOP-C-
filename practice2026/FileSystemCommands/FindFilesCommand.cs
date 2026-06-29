using System;
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
        // Заглушка: пока ничего не делает
    }
}