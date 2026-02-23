using Grievance_Management_System.AppDbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Grievance_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController (GrievenceDbContext mContext) : ControllerBase
    {
        [HttpGet ("getAllStudents")]

        public IActionResult GetAllStudents()
        {
            return Ok(mContext.Students.ToList());
        }
    }
}
