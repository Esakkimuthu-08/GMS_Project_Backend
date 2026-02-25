using Grievance_Management_System.AppDbContext;
using Grievance_Management_System.Constants;
using Grievance_Management_System.Enum;
using Grievance_Management_System.Model;
using Grievance_Management_System.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Grievance_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController(GrievenceDbContext mContext) : ControllerBase
    {
        //private readonly GrievenceDbContext mContext;
        //public StaffController(GrievenceDbContext grievenceDbContext) 
        //{
        //    mContext = grievenceDbContext;
        //}

        [Authorize(Roles = "Admin")]
        [HttpPost("createStaff")]

        public IActionResult CreateStaff([FromBody] StaffRequest staffRequest)
        {
            if ( string.IsNullOrEmpty(staffRequest.Name) || staffRequest.Email == null)
            {
                return BadRequest(ErrorConstant.BadRequest);
            }
            if(staffRequest.PhoneNumber.Length != 10)
            {
                return BadRequest("Phone Number must be 10 Digit");
            }

            Staff staff = new Staff()
            {
                StaffCode = staffRequest.StaffCode,
                Name = staffRequest.Name,
                Email = staffRequest.Email,
                Role = RoleEnum.Staff,
                PhoneNumber = staffRequest.PhoneNumber,
            };
            mContext.Staffs.Add(staff);
            mContext.SaveChanges();
            return Ok(ErrorConstant.Created);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("getAllStaff")]

        public IActionResult GetStaffList()
        {
            return Ok(mContext.Staffs.ToList());
        }
    }
}
