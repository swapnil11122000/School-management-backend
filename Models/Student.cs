using System.Security.Claims;

namespace TestbackendProject.Models
{
    public class Student
    {
        public int StudentID { get; set; } // Primary Key
        public int RollNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ClassID { get; set; } // Foreign Key
        public string House { get; set; } = string.Empty; // ENUM ('Blue', 'Green', 'Red', 'Yellow')
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // Navigation Properties
        public Class Class { get; set; } = null!;
        public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
        public SeatingPlan? SeatingPlan { get; set; }
    }
}
