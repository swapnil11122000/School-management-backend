namespace TestbackendProject.Models
{
    public class Subject
    {
        public int SubjectID { get; set; } // Primary Key
        public string SubjectName { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
        public ICollection<Score> Scores { get; set; } = new List<Score>();
    }
}
