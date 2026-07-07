using Xunit;
using task05;

public class TestClass
{
    public int PublicField;
    private string? _privateField;
    public int Property { get; set; }

    public void Method(string name, int age) { }
}

[Serializable]
public class AttributedClass { }

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
    }
    
    [Fact]
    public void GetMethodParams_ReturnsCorrectParams()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var parameters = analyzer.GetMethodParams("Method");

        Assert.Contains("name", parameters);
        Assert.Contains("age", parameters);
    }

    [Fact]
    public void GetProperties_IncludesProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var props = analyzer.GetProperties();

        Assert.Contains("Property", props);
    }

    [Fact]
    public void HasAttribute_ReturnsCorrectAttribute()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));

        Assert.True(analyzer.HasAttribute<SerializableAttribute>());
    }
}
