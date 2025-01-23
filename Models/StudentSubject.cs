namespace TestbackendProject.Models
{
    public class StudentSubject
    {
        public int StudentID { get; set; } // Composite Key
        public int SubjectID { get; set; } // Composite Key

        // Navigation Properties
        public Student Student { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
    }
}
