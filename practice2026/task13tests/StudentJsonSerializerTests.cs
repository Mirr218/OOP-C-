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
    public void SerializeReurnsCustomBirthDate()
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
    public void DeserializeReturnsCustomBirthDate()
    {
        var json = "{\"firstName\":\"John\",\"lastName\":\"Doe\",\"birthDate\":\"1990-01-01\"}";
        var student = StudentJsonSerializer.Deserialize(json);
        Assert.NotNull(student);
        Assert.Equal(new DateTime(1990, 1, 1), student.BirthDate);
    }
}