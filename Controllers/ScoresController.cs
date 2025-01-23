using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestbackendProject.Models;

namespace TestbackendProject.Controllers
{
    [Route("api/scores")]
    [ApiController]
    public class ScoresController : ControllerBase
    {
        private readonly ExamManagementContext _context;

        public ScoresController(ExamManagementContext context)
        {
            _context = context;
        }

        // Add score for a student in a subject
        [HttpPost("add")]
        public async Task<IActionResult> AddScore([FromBody] AddScoreRequest request)
        {
            // Check if the student is enrolled in the subject
            var studentSubject = await _context.StudentSubjects
                .FirstOrDefaultAsync(ss => ss.StudentID == request.StudentID && ss.SubjectID == request.SubjectID);

            if (studentSubject == null)
            {
                return BadRequest(new { message = "Student is not enrolled in this subject." });
            }

            // Create new score object
            var score = new Score
            {
                StudentID = request.StudentID,
                SubjectID = request.SubjectID,
                ScoreValue = request.ScoreValue
            };

            // Add score to database and save
            _context.Scores.Add(score);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Score added successfully." });
        }

        // Get all subjects for a student
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetSubjectsForStudent(int studentId)
        {
            var studentSubjects = await _context.StudentSubjects
                .Where(ss => ss.StudentID == studentId)
                .Include(ss => ss.Subject)
                .Select(ss => new
                {
                    ss.Subject.SubjectID,
                    ss.Subject.SubjectName
                })
                .ToListAsync();

            return Ok(studentSubjects);
        }
    }

    // Request model to add score
    public class AddScoreRequest
    {
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public decimal ScoreValue { get; set; }
    }
}
