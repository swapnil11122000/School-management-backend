namespace TestbackendProject.Models
{
    public class SeatingPlan
    {
        public int SeatingID { get; set; } // Primary Key
        public int ClassroomID { get; set; } // Foreign Key
        public int Row { get; set; }
        public int Column { get; set; }
        public int StudentID { get; set; } // Foreign Key

        // Navigation Properties
        public Classroom Classroom { get; set; } = null!;
        public Student Student { get; set; } = null!;
    }

}
