namespace task02;

public class Student
{
    public string Name { get; set; } = string.Empty;
    public string Faculty { get; set; } = string.Empty;
    public List<int> Grades { get; set; } = new List<int>();
}

public class StudentService
    {
        private readonly List<Student> _students;

        public StudentService(List<Student> students) => _students = students;

        // 1. Возвращает студентов указанного факультета
        public IEnumerable<Student> GetStudentsByFaculty(string faculty)
            => new List<Student>(); // Заглушка

        // 2. Возвращает студентов со средним баллом >= minAverageGrade
        public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
            => new List<Student>(); // Заглушка

        // 3. Возвращает студентов, отсортированных по имени (A-Z)
        public IEnumerable<Student> GetStudentsOrderedByName()
            => new List<Student>(); // Заглушка

        // 4. Группировка по факультету
        public ILookup<string, Student> GroupStudentsByFaculty()
            => new List<Student>().ToLookup(s => s.Faculty);

        // 5. Находит факультет с максимальным средним баллом
        public string GetFacultyWithHighestAverageGrade()
            => ""; // Заглушка
    }