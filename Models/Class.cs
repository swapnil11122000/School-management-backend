namespace TestbackendProject.Models
{
    public class Class
    {
        public int ClassID { get; set; } // Primary Key
        public string ClassName { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
