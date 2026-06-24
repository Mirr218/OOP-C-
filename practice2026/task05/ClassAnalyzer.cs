using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace task05;

public class ClassAnalyzer
{
    private Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }

    public IEnumerable<string> GetPublicMethods()
    {
        return _type.GetMethods()
            .Select(m => m.Name);
    }

    public IEnumerable<string> GetMethodParams(string methodName)
    {
        var method = _type.GetMethod(methodName);
        if (method == null)
            return Enumerable.Empty<string>();

        return method.GetParameters().Select(p => p.Name!);
    }

    public IEnumerable<string> GetAllFields()
    {
        return _type.GetFields(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        ).Select(f => f.Name);
    }

    public IEnumerable<string> GetProperties()
    {
        return _type.GetProperties()
            .Select(p => p.Name);
    }

    public bool HasAttribute<T>() where T : Attribute
    {
        return _type.IsDefined(typeof(T), false);
    }
}