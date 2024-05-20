using Microsoft.AspNetCore.Mvc;
using StudentManagement_BYMongoDB.Models;
using StudentManagement_BYMongoDB.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentManagement_BYMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentServices studentService;

        public StudentController(IStudentServices studentService)
        {
            this.studentService = studentService;
        }
        // GET: api/<StudentController>
        [HttpGet]
        public ActionResult<List<Students>> Get()
        {
            return studentService.Get();
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public ActionResult<Students> Get(string id)
        {
            return studentService.Get(id);
        }
        
        [HttpGet("GetGraduatedStudents")]
        public ActionResult<List<Students>> GetGraduatedStudents()
        {
            return studentService.GetGraduatedStudents();
        }   
        [HttpGet("GetStudentsDetailsOnly")]
        public ActionResult<List<dynamic>> GetStudentsDetailsOnly()
        {
            return (List<dynamic>)studentService.GetStudentsDetailsOnly();
        }

        // POST api/<StudentController>
        [HttpPost]
        public ActionResult<Students> Post([FromBody] Students student)
        {
             studentService.Create(student);

            return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
        }     
        
        // POST api/<StudentController>
        [HttpPost("InsertStudentsData")]
        public ActionResult<Students> InsertStudentsData([FromBody] List<Students> students)
        {
             studentService.CreateMultipleStudents(students);

            return CreatedAtAction(nameof(Get), students);
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Students student)
        {
            var existingStudent = studentService.Get(id);

            if(existingStudent == null)
            {
                return NotFound($"Student with Id = {id} not Found");
            }

            studentService.Update(id, student);
            return NoContent();
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var existingStudent = studentService.Get(id);

            if (existingStudent == null)
            {
                return NotFound($"Student with Id = {id} not Found");
            }

            studentService.Remove(existingStudent.Id);
            return Ok($"Student With Id = {id} deleted");
        }
    }
}
