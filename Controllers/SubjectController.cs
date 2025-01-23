using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestbackendProject.Models;

namespace TestbackendProject.Controllers
{
    [Route("api/subjects")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ExamManagementContext _context;

        public SubjectController(ExamManagementContext context)
        {
            _context = context;
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollToSubject([FromBody] EnrollSubjectRequest request)
        {
            var student = await _context.Students.FindAsync(request.StudentID);
            var subject = await _context.Subjects.FindAsync(request.SubjectID);

            if (student == null || subject == null)
            {
                return NotFound(new { message = "Invalid student or subject ID." });
            }

            _context.StudentSubjects.Add(new StudentSubject
            {
                StudentID = request.StudentID,
                SubjectID = request.SubjectID
            });

            await _context.SaveChangesAsync();
            return Ok(new { message = "Student enrolled in subject successfully." });
        }

        [HttpPost("score")]
        public async Task<IActionResult> AddScore([FromBody] AddScoreRequest request)
        {
            var student = await _context.Students.FindAsync(request.StudentID);
            var subject = await _context.Subjects.FindAsync(request.SubjectID);

            if (student == null || subject == null)
            {
                return NotFound(new { message = "Invalid student or subject ID." });
            }

            _context.Scores.Add(new Score
            {
                StudentID = request.StudentID,
                SubjectID = request.SubjectID,
                ScoreValue = request.ScoreValue
            });

            await _context.SaveChangesAsync();
            return Ok(new { message = "Score added successfully." });
        }
        [HttpGet("GetEnrolledSubjects/{studentId}")]
        public async Task<IActionResult> GetEnrolledSubjects(int studentId)
        {
            // Find the student
            var student = await _context.Students
                .Include(s => s.StudentSubjects) // Include related StudentSubjects
                .ThenInclude(ss => ss.Subject) // Include the Subject information
                .FirstOrDefaultAsync(s => s.StudentID == studentId);

            // If student not found
            if (student == null)
            {
                return NotFound(new { message = "Student not found." });
            }

            // Select the subject names
            var enrolledSubjects = student.StudentSubjects
                .Select(ss => ss.Subject.SubjectName) // Assuming 'Name' is the property for subject name
                .ToList();

            return Ok(enrolledSubjects);
        }
        [HttpGet("GetAllSubjects")]
        public async Task<IActionResult> GetAllSubjects()
        {
            // Retrieve all subjects from the database
            var subjects = await _context.Subjects
                .Select(s => new
                {
                    s.SubjectID,
                    s.SubjectName
                })
                .ToListAsync();

            // If no subjects found, return a 404 not found
            if (subjects.Count == 0)
            {
                return NotFound(new { message = "No subjects found." });
            }

            return Ok(subjects); // Return all subjects
        }
    }
}
