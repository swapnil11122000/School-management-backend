using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TestbackendProject.Models;
using SolrNet;
using SolrNet.Commands.Parameters;
using Microsoft.EntityFrameworkCore;

namespace TestbackendProject.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ExamManagementContext _context;
        private readonly ISolrOperations<Student> _solrOperations;

        public StudentController(ExamManagementContext context, ISolrOperations<Student> solrOperations)
        {
            _context = context;
            _solrOperations = solrOperations;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _context.Students
                .Include(s => s.Class) // Include related data
                .Select(s => new
                {
                    s.StudentID,
                    s.Name,
                    s.RollNumber,
                    ClassName = s.Class.ClassName, // Access Class property
                    s.House
                })
                .ToListAsync(); // Materialize the result

            var studentDTOs = students.Select(s => new StudentDTO
            {
                StudentID = s.StudentID,
                Name = s.Name,
                RollNumber = s.RollNumber,
                ClassName = s.ClassName,
                House = s.House
            }).ToList();

            return Ok(studentDTOs);
        }

        [HttpGet("getByName")]
        public async Task<IActionResult> GetStudentByName([FromQuery] string name)
        {
            // Fetch the student with the provided name
            var student = await _context.Students
                .Include(s => s.Class) // Include related Class data
                .Where(s => s.Name == name)
                .Select(s => new
                {
                    s.StudentID,
                    s.Name,
                    s.RollNumber,
                    ClassName = s.Class.ClassName, // Access Class property
                    s.House
                })
                .FirstOrDefaultAsync(); // Fetch the first matching student or null

            // If student not found, return 404 NotFound
            if (student == null)
            {
                return NotFound(new { message = "Student not found." });
            }

            // Return the student data
            var studentDTO = new StudentDTO
            {
                StudentID = student.StudentID,
                Name = student.Name,
                RollNumber = student.RollNumber,
                ClassName = student.ClassName,
                House = student.House
            };

            return Ok(studentDTO);
        }



        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            // Fetch the student with the provided studentId
            var student = await _context.Students
                .Include(s => s.Class) // Include related Class data
                .Where(s => s.StudentID == studentId)
                .Select(s => new
                {
                    s.StudentID,
                    s.Name,
                    s.RollNumber,
                    ClassName = s.Class.ClassName, // Access Class property
                    s.House
                })
                .FirstOrDefaultAsync(); // Fetch the first matching student or null

            // If student not found, return 404 NotFound
            if (student == null)
            {
                return NotFound(new { message = "Student not found." });
            }

            // Return the student data
            var studentDTO = new StudentDTO
            {
                StudentID = student.StudentID,
                Name = student.Name,
                RollNumber = student.RollNumber,
                ClassName = student.ClassName,
                House = student.House
            };

            return Ok(studentDTO);
        }
    }
}
