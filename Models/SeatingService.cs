using Microsoft.EntityFrameworkCore;

namespace TestbackendProject.Models
{
    public class SeatingService
    {
        private readonly ExamManagementContext _context;
  

        public SeatingService(ExamManagementContext context)
        {
            _context = context;
        }

        // Rule 1: Validate if seating is available based on row/column constraints and same class
        public async Task<bool> IsSeatingValidAsync(int classroomId, int row, int column, int studentId)
        {
            // Retrieve student details with related class
            var student = await _context.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.StudentID == studentId);

            if (student == null)
                throw new ArgumentException("Invalid StudentID");

            // Retrieve existing seating for the classroom to check current seating layout
            var existingSeats = await _context.SeatingPlans
                .Include(sp => sp.Student)
                .Where(sp => sp.ClassroomID == classroomId)
                .ToListAsync();

            // Rule 1: No two students from the same class in the same row or column
            if (existingSeats.Any(sp =>
                    (sp.Row == row || sp.Column == column) &&
                    sp.Student.ClassID == student.ClassID))
            {
                return false; // Violates Rule 1
            }

            // Rule 2: No two students from the same house should be adjacent
            var adjacentSeats = existingSeats.Where(sp =>
                (Math.Abs(sp.Row - row) <= 1 && Math.Abs(sp.Column - column) <= 1));

            if (adjacentSeats.Any(sp => sp.Student.House == student.House))
            {
                return false; // Violates Rule 2
            }

            return true; // Seating is valid
        }

        // Rule 1 & 2 validation + Assign seat logic
        public async Task<bool> AssignSeatAsync(int classroomId, int row, int column, int studentId)
        {
            // Ensure the seat is valid before assigning it
            if (await IsSeatingValidAsync(classroomId, row, column, studentId))
            {
                // Create and save the seating plan
                var seatingPlan = new SeatingPlan
                {
                    ClassroomID = classroomId,
                    Row = row,
                    Column = column,
                    StudentID = studentId
                };

                // Add seating plan to the context and save changes
                _context.SeatingPlans.Add(seatingPlan);
                await _context.SaveChangesAsync();
                return true;
            }

            return false; // Invalid seating attempt
        }

        // To retrieve the seating plan for a classroom
        public async Task<List<SeatingPlanDTO>> GetSeatingPlanAsync(int classroomId)
        {
            // Retrieve seating plan for the given classroom
            var seatingPlans = await _context.SeatingPlans
                .Where(sp => sp.ClassroomID == classroomId)
                .Include(sp => sp.Student)
                .ToListAsync();

            // Map to DTO for response
            var seatingPlanDTOs = seatingPlans.Select(sp => new SeatingPlanDTO
            {
                Row = sp.Row,
                Column = sp.Column,
                StudentID = sp.StudentID,
                StudentName = sp.Student.Name
            }).ToList();

            return seatingPlanDTOs;
        }

        public async Task<SeatingPlanDTO2?> GetSeatAllocationAsync(int studentId)
        {
            // Retrieve seating information for the given student
            var seatingPlan = await _context.SeatingPlans
                .Include(sp => sp.Student)
                .FirstOrDefaultAsync(sp => sp.StudentID == studentId);

            // If no seat allocation is found, return null
            if (seatingPlan == null)
            {
                return null;
            }

            // Map to DTO for response
            var seatingPlanDTO = new SeatingPlanDTO2
            {
                Row = seatingPlan.Row,
                Column = seatingPlan.Column,
                StudentID = seatingPlan.StudentID,
                StudentName = seatingPlan.Student.Name,
                ClassroomID = seatingPlan.ClassroomID

            };

            return seatingPlanDTO;
        }

    }
}

public class SeatingPlanDTO2
{
    public int Row { get; set; }
    public int Column { get; set; }
    public int StudentID { get; set; }
    public string StudentName { get; set; }
    public int ClassroomID { get; set; }  // Add ClassroomID property
}
