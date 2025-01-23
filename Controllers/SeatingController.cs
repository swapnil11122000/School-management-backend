using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestbackendProject.Models;

namespace TestbackendProject.Controllers
{
    [Route("api/seating")]
    [ApiController]
    public class SeatingController : ControllerBase
    {
        private readonly SeatingService _seatingService;

        public SeatingController(SeatingService seatingService)
        {
            _seatingService = seatingService;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignSeat([FromBody] AssignSeatRequest request)
        {
            if (await _seatingService.AssignSeatAsync(request.ClassroomID, request.Row, request.Column, request.StudentID))
            {
                return Ok(new { message = "Seat assigned successfully." });
            }

            return BadRequest(new { message = "Invalid seating assignment. Check seating rules." });
        }

        [HttpGet("{classroomId}")]
        public async Task<IActionResult> GetSeatingPlan(int classroomId)
        {
            var seatingPlan = await _seatingService.GetSeatingPlanAsync(classroomId);
            return Ok(seatingPlan);
        }

        [HttpGet("GetSeatAllocation/{studentId}")]
        public async Task<IActionResult> GetSeatAllocation(int studentId)
        {
            var seatAllocation = await _seatingService.GetSeatAllocationAsync(studentId);

            if (seatAllocation == null)
            {
                return NotFound($"No seat allocation found for student ID {studentId}");
            }

            return Ok(seatAllocation);
        }

    }
}

public class AssignSeatRequest
{
    public int ClassroomID { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public int StudentID { get; set; }
}