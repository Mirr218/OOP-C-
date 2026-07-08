using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace task13;

public class StudentJsonSerializer
{

    public static string Serialize(Student student)
    {
        return JsonSerializer.Serialize(student, new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new CustomDateConverter() }
        });
    }
}
