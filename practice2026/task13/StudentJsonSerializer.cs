using System.Text.Json;

namespace task13;

public class StudentJsonSerializer
{

    public static string Serialize(Student student)
    {
        return JsonSerializer.Serialize(student);
    }
}
