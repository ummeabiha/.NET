using LearnDapper.Model;
using LearnDapper.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepo repo;

        public StudentController(IStudentRepo repo)
        {
            this.repo = repo;
        }

        [HttpGet("GetAll")]

        public async Task <IActionResult> GetAll()
        {
            var _list= await this.repo.GetAll();
            if(_list!= null)
            {
                return Ok(_list);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpGet("GetById")]

        public async Task<IActionResult> GetById(int student_Id)
        {
            var _list = await this.repo.GetById(student_Id);
            if (_list != null)
            {
                return Ok(_list);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost("AddRecord")]

        public async Task<IActionResult> AddRecord([FromBody] Students student)
        {
            var _result = await this.repo.AddRecord(student);
            return Ok(_result);
            
        }


        [HttpPut("UpdateRecord")]

        public async Task<IActionResult> UpdateRecord([FromBody] Students student, int student_Id)
        {
            var _result = await this.repo.UpdateRecord(student, student_Id);
            return Ok(_result);

        }


        [HttpPatch("UpdateRecordUsingPatch")]

        public async Task<IActionResult> UpdateRecordUsingPatch([FromBody] Students student, int student_Id)
        {
            var _result = await this.repo.UpdateRecordUsingPatch(student, student_Id);
            return Ok(_result);

        }


        [HttpDelete("DeleteRecord")]

        public async Task<IActionResult> DeleteRecord(int student_Id)
        {
            var _result = await this.repo.DeleteRecord(student_Id);
            if (_result != null)
            {
                return Ok(_result);
            }
            else
            {
                return NotFound();
            }

        }
    }
}
