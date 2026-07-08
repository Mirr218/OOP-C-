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

    public static Student Deserialize(string json)
    {
        var student = JsonSerializer.Deserialize<Student>(json, CreateOptions());

        Validate(student);

        return student!;
    }

    public static void SaveToFile(Student student, string path)
    {
        var json = Serialize(student);
        File.WriteAllText(path, json);
    }

    public static Student LoadFromFile(string path)
    {
        var json = File.ReadAllText(path);
        return Deserialize(json);
    }

    private static void Validate(Student? student)
    {
        if (student is null)
        {
            throw new InvalidOperationException("Invalid student data.");
        }

        if (string.IsNullOrWhiteSpace(student.FirstName))
        {
            throw new InvalidOperationException("Invalid student data.");
        }

        if (string.IsNullOrWhiteSpace(student.LastName))
        {
            throw new InvalidOperationException("Invalid student data.");
        }

        if (student.BirthDate is null)
        {
            throw new InvalidOperationException("Invalid student data.");
        }

        if (student.Grades is null || student.Grades.Count == 0)
        {
            throw new InvalidOperationException("Invalid student data.");
        }

        foreach (var subject in student.Grades)
        {
            if (string.IsNullOrWhiteSpace(subject.Name))
            {
                throw new InvalidOperationException("Invalid student data.");
            }

            if (subject.Grade is null || subject.Grade < 0 || subject.Grade > 100)
            {
                throw new InvalidOperationException("Invalid student data.");
            }
        }
    }
}
