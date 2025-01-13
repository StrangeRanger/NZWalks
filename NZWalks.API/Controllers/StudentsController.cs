using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllStudents()
    {
        string[] studentsNames = new[] { "John", "Jane", "Jack", "Jill" };

        return Ok(studentsNames);
    }
}