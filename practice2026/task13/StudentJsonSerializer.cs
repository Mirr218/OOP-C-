using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace task13;

public class StudentJsonSerializer
{
    private static JsonSerializerOptions CreateOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new CustomDateConverter() }
        };
    }

    public static string Serialize(Student student)
    {
        return JsonSerializer.Serialize(student, CreateOptions());
    }

    public static Student? Deserialize(string json)
    {
        return JsonSerializer.Deserialize<Student>(json, CreateOptions());
    }
}
