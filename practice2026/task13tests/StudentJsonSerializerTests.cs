using System.Text.Json;
using Xunit;
using task13;

namespace task13tests;

public class StudentJsonSerializerTests
{

    [Fact]
    public void FirstNameIsNullReturnsWithoutFirstName()
    {
        var student = new Student{
            FirstName = null,
            LastName = "Doe",
            BirthDate = new DateTime(1990, 1, 1),
        };

        var json = StudentJsonSerializer.Serialize(student);
        Assert.DoesNotContain("firstName", json);
        Assert.Contains("lastName", json);
    }

    [Fact]
    public void LastNameIsNullReturnsWithoutLastName()
    {
        var student = new Student{
            FirstName = "John",
            LastName = null,
            BirthDate = new DateTime(1990, 1, 1),
            Grades = new List<Subject>{
                new Subject{Name = "Math", Grade = 90},
                new Subject{Name = "English", Grade = 85},
            }
        };

        var json = StudentJsonSerializer.Serialize(student);
        Assert.DoesNotContain("lastName", json);
        Assert.Contains("firstName", json);
    }

    [Fact]
    public void BirthDateIsNullReturnsWithoutBirthDate()
    {
        var student = new Student{
            FirstName = "John",
            LastName = "Doe",
            BirthDate = null,
            Grades = new List<Subject>{
                new Subject{Name = "Math", Grade = 90},
                new Subject{Name = "English", Grade = 85},
            }
        };

        var json = StudentJsonSerializer.Serialize(student);
        Assert.DoesNotContain("birthDate", json);
        Assert.Contains("lastName", json);
    }

    [Fact]
    public void GradesIsNullReturnsWithoutGrades()
    {
        var student = new Student{
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1990, 1, 1),
            Grades = null,
        };

        var json = StudentJsonSerializer.Serialize(student);
        Assert.DoesNotContain("grades", json);
        Assert.Contains("lastName", json);
    }

    [Fact]
    public void SerializeReturnsCustomBirthDate()
    {
        var student = new Student{
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1990, 1, 1),
        };

        var json = StudentJsonSerializer.Serialize(student);
        Assert.Contains("\"birthDate\":\"1990-01-01\"", json);
        Assert.DoesNotContain("T00:00:00", json);
    }

    [Fact]
    public void DeserializeReturnsStudent()
    {
        var json = "{\"firstName\":\"John\",\"lastName\":\"Doe\",\"birthDate\":\"1990-01-01\",\"grades\":[{\"name\":\"Math\",\"grade\":90},{\"name\":\"English\",\"grade\":85}]}";

        var student = StudentJsonSerializer.Deserialize(json);

        Assert.NotNull(student);
        Assert.Equal("John", student.FirstName);
        Assert.Equal("Doe", student.LastName);
        Assert.Equal(new DateTime(1990, 1, 1), student.BirthDate);
        Assert.NotNull(student.Grades);
        Assert.Equal(2, student.Grades.Count);
        Assert.Equal("Math", student.Grades[0].Name);
        Assert.Equal(90, student.Grades[0].Grade);
    }

    [Fact]
    public void DeserializeWithEmptyFirstNameThrowsException()
    {
        var json = "{\"firstName\":\"\",\"lastName\":\"Doe\",\"birthDate\":\"1990-01-01\",\"grades\":[{\"name\":\"Math\",\"grade\":90}]}";

        Assert.Throws<InvalidOperationException>(() => StudentJsonSerializer.Deserialize(json));
    }

    [Fact]
    public void DeserializeWithEmptyLastNameThrowsException()
    {
        var json = "{\"firstName\":\"John\",\"lastName\":\"\",\"birthDate\":\"1990-01-01\",\"grades\":[{\"name\":\"Math\",\"grade\":90}]}";

        Assert.Throws<InvalidOperationException>(() => StudentJsonSerializer.Deserialize(json));
    }

    [Fact]
    public void DeserializeWithoutBirthDateThrowsException()
    {
        var json = "{\"firstName\":\"John\",\"lastName\":\"Doe\",\"grades\":[{\"name\":\"Math\",\"grade\":90}]}";

        Assert.Throws<InvalidOperationException>(() => StudentJsonSerializer.Deserialize(json));
    }

    [Fact]
    public void DeserializeWithoutGradesThrowsException()
    {
        var json = "{\"firstName\":\"John\",\"lastName\":\"Doe\",\"birthDate\":\"1990-01-01\"}";

        Assert.Throws<InvalidOperationException>(() => StudentJsonSerializer.Deserialize(json));
    }

    [Fact]
    public void DeserializeWithEmptySubjectNameThrowsException()
    {
        var json = "{\"firstName\":\"John\",\"lastName\":\"Doe\",\"birthDate\":\"1990-01-01\",\"grades\":[{\"name\":\"\",\"grade\":90}]}";

        Assert.Throws<InvalidOperationException>(() => StudentJsonSerializer.Deserialize(json));
    }

    [Fact]
    public void DeserializeWithInvalidGradeThrowsException()
    {
        var json = "{\"firstName\":\"John\",\"lastName\":\"Doe\",\"birthDate\":\"1990-01-01\",\"grades\":[{\"name\":\"Math\",\"grade\":101}]}";

        Assert.Throws<InvalidOperationException>(() => StudentJsonSerializer.Deserialize(json));
    }

    [Fact]
    public void SaveToFileWritesJson()
    {
        var path = Path.GetTempFileName();
        var student = new Student{
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1990, 1, 1),
            Grades = new List<Subject>{
                new Subject{Name = "Math", Grade = 90}
            }
        };

        try
        {
            StudentJsonSerializer.SaveToFile(student, path);

            var json = File.ReadAllText(path);
            Assert.Contains("\"firstName\":\"John\"", json);
            Assert.Contains("\"birthDate\":\"1990-01-01\"", json);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void LoadFromFileReturnsStudent()
    {
        var path = Path.GetTempFileName();
        var json = "{\"firstName\":\"John\",\"lastName\":\"Doe\",\"birthDate\":\"1990-01-01\",\"grades\":[{\"name\":\"Math\",\"grade\":90}]}";

        try
        {
            File.WriteAllText(path, json);

            var student = StudentJsonSerializer.LoadFromFile(path);

            Assert.Equal("John", student.FirstName);
            Assert.Equal(new DateTime(1990, 1, 1), student.BirthDate);
            Assert.NotNull(student.Grades);
            Assert.Single(student.Grades);
            Assert.Equal("Math", student.Grades[0].Name);
        }
        finally
        {
            File.Delete(path);
        }
    }
}