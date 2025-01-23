namespace TestbackendProject.Models
{
    public class Classroom
    {
        public int ClassroomID { get; set; } // Primary Key
        public string ClassroomName { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<SeatingPlan> SeatingPlans { get; set; } = new List<SeatingPlan>();
    }
}
