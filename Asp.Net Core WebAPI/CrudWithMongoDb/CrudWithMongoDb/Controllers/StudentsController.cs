using CrudWithMongoDb.Entities;
using CrudWithMongoDb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrudWithMongoDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentsController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = studentService.GetStudents();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            var student = studentService.GetStudent(id);
            return Ok(student);
        }

        [HttpPost]
        public IActionResult CreateStudent([FromBody] Student student)
        {
            studentService.Create(student);
            return Created("", student);
        }

        [HttpPut]
        public IActionResult UpdateStudent([FromBody] Student student)
        {
            studentService.Update(student.Id, student);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(string id)
        {
            studentService.Remove(id);
            return NoContent();
        }
    }
}
