using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Required for async methods like FirstOrDefaultAsync
using System.Text;
using TestbackendProject.Models; // Ensure you're using the correct LoginRequest
using System.Security.Cryptography;
namespace TestbackendProject.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ExamManagementContext _context;

        public AuthenticationController(ExamManagementContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] TestbackendProject.Models.LoginRequest request)  // Fully qualify LoginRequest
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Username == request.Username && s.PasswordHash == request.PasswordHash);

            if (student == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(new { message = "Login successful.", studentID = student.StudentID });
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower(); // Return the hashed password as a hex string
            }
        }
    }
}
