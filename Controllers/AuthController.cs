using Grievance_Management_System.AppDbContext;
using Grievance_Management_System.Auth;
using Grievance_Management_System.Constants;
using Grievance_Management_System.Model;
using Grievance_Management_System.Model.Auth;
using Grievance_Management_System.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Grievance_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(GrievenceDbContext mContext) : ControllerBase
    {
        [HttpPost("StaffSignUp")]
        public IActionResult StaffSignUp([FromBody] StaffSignUpRequest request)
        {
            if (string.IsNullOrEmpty(request.Name)
                || string.IsNullOrEmpty(request.Email)
                || string.IsNullOrEmpty(request.PasswordHash))
            {
                return BadRequest(ErrorConstant.BadRequest);
            }

            User user = mContext.Users.FirstOrDefault(x => x.Email == request.Email);

            if (user != null)
            {
                if (user.IsActive == false)
                    return BadRequest(ErrorConstant.AccountDisabled);

                return BadRequest(ErrorConstant.AccountExist);
            }

            bool pendingRequest = mContext.StaffSignUp.Any(staff => staff.Email == request.Email && staff.IsApproved == false);

            if (pendingRequest)
            {
                return BadRequest(ErrorConstant.AccountPending);
            }
           
            StaffSignUp staffSignUp = new StaffSignUp()
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                IsApproved = false
            };
            mContext.StaffSignUp.Add(staffSignUp);
            mContext.SaveChanges();
            return Ok(ErrorConstant.Created);
        }

        [HttpGet ("getAllStaffSignUpRequest")]

        public IActionResult getAllStaffSignUpRequest()
        {
            return Ok(mContext.StaffSignUp.ToList());
        }

        [HttpPost ("Student-SignUp")]

        public IActionResult StudentSignUp([FromBody] StudentSignUpRequest studentSignUpRequest)
        {
            if(string.IsNullOrEmpty(studentSignUpRequest.Name) ||
                string.IsNullOrEmpty(studentSignUpRequest.Email) ||
                string.IsNullOrEmpty(studentSignUpRequest.PasswordHash) ||
                string.IsNullOrEmpty(studentSignUpRequest.StaffCode)
                )
            {
                return BadRequest(ErrorConstant.BadRequest);
            }

            User user = mContext.Users.Where(user => user.Email ==  studentSignUpRequest.Email).FirstOrDefault();

            if(user != null)
            {
                if(user.IsActive == false )                
                    return BadRequest(ErrorConstant.AccountDisabled);
                
                return BadRequest(ErrorConstant.AccountExist);
            }

            bool PendingRequest = mContext.StudentSignUp.Any(request => request.Email == studentSignUpRequest.Email && request.IsApproved == false);
            
            if (PendingRequest)
            {
                return BadRequest(ErrorConstant.AccountPending);
            }
            
            Staff staffCode = mContext.Staffs.Where(code => code.StaffCode == studentSignUpRequest.StaffCode).FirstOrDefault();
            
            if(staffCode == null)
            {
                return NotFound(ErrorConstant.NotFound);
            }

            StudentSignUp studentSignUp = new StudentSignUp()
            {
                RollNo = studentSignUpRequest.RollNo,
                Name = studentSignUpRequest.Name,
                Email = studentSignUpRequest.Email,
                PasswordHash = studentSignUpRequest.PasswordHash,
                StaffCode = staffCode.StaffCode,
                IsApproved = false,
            };
            mContext.StudentSignUp.Add(studentSignUp);
            mContext.SaveChanges();
            return Ok(ErrorConstant.Created);

        }

        [HttpGet("getAllStudentSignUpRequest")]

        public IActionResult GetAllStudentSignUpRequest()
        {
            return Ok(mContext.StudentSignUp.ToList());
        }
    }
}
