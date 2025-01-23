namespace TestbackendProject.Models
{
    public class Score
    {
        public int ScoreID { get; set; } // Primary Key
        public int StudentID { get; set; } // Foreign Key
        public int SubjectID { get; set; } // Foreign Key
        public decimal ScoreValue { get; set; }

        // Navigation Properties
        public Student Student { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
    }
}
