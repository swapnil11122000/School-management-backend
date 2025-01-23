using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestbackendProject.Models;
using Microsoft.EntityFrameworkCore; // Add this line

namespace TestbackendProject.Controllers
{
    [Route("api/classes")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly ExamManagementContext _context;

        public ClassController(ExamManagementContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClasses()
        {
            var classes = await _context.Classes.ToListAsync();
            return Ok(classes);
        }
    }
}
