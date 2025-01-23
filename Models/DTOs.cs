namespace TestbackendProject.Models
{
    public class DTOs
    {
    }
    public class StudentDTO
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public int RollNumber { get; set; }
        public string ClassName { get; set; }
        public string House { get; set; }
    }

    public class AssignSeatRequest
    {
        public int ClassroomID { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int StudentID { get; set; }
    }

    public class EnrollSubjectRequest
    {
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
    }
    public class SeatingPlanDTO
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
    }

    public class AddScoreRequest
    {
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public decimal ScoreValue { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }

}
