using Grievance_Management_System.AppDbContext;
using Grievance_Management_System.Auth;
using Grievance_Management_System.Constants;
using Grievance_Management_System.Enum;
using Grievance_Management_System.Model;
using Grievance_Management_System.Model.Auth;
using Grievance_Management_System.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Grievance_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalController(GrievenceDbContext mContext) : ControllerBase
    {
        [HttpPost("approve-staff")]
        public IActionResult ApproveStaff([FromBody] ApproveStaffDto dto)
        {

            StaffSignUp signupRequest = mContext.StaffSignUp
                .FirstOrDefault(x => x.Id == dto.SignupRequestId);

            if (signupRequest == null)
                return NotFound(ErrorConstant.NotFound);

            if (signupRequest.IsApproved)
                return BadRequest(ErrorConstant.AlreadyApproved); 

            bool userExists = mContext.Users.Any(u => u.Email == signupRequest.Email);

            if (userExists)
                return BadRequest(ErrorConstant.AccountExist);

            User user = new User
            {
                Email = signupRequest.Email,
                PasswordHash = signupRequest.PasswordHash,
                Role = dto.Role,
                IsActive = true
            };
            mContext.Users.Add(user);

            Staff staff = new Staff
            {
                Name = signupRequest.Name,
                Email = signupRequest.Email,
                Role = dto.Role,
                StaffCode = dto.StaffCode,
                PhoneNumber = dto.PhoneNumber
            };

            mContext.Staffs.Add(staff);
            signupRequest.IsApproved = true;
            mContext.SaveChanges();

            return Ok(ErrorConstant.Approved);
        }
        [HttpPost("approve-student")]
        public IActionResult ApproveStudent([FromBody] ApproveStudentDto dto)
        {
            var signUpRequest = mContext.StudentSignUp
                .FirstOrDefault(x => x.Id == dto.StudentSignUpId);

            if (signUpRequest == null)
                return NotFound(ErrorConstant.NotFound);

            if (signUpRequest.IsApproved)
                return BadRequest(ErrorConstant.AlreadyApproved);

            var staff = mContext.Staffs
                .FirstOrDefault(x => x.StaffCode == signUpRequest.StaffCode);

            if (staff == null)
                return BadRequest(ErrorConstant.NotFound);

            bool userExists = mContext.Users.Any(u => u.Email == signUpRequest.Email);

            if (userExists)
                return BadRequest(ErrorConstant.AccountExist);

            var user = new User
            {
                Email = signUpRequest.Email,
                PasswordHash = signUpRequest.PasswordHash,
                Role = RoleEnum.Student,
                IsActive = true
            };
            mContext.Users.Add(user);

            var student = new Student
            {
                RollNo = signUpRequest.RollNo, 
                Name = signUpRequest.Name,
                Email = signUpRequest.Email,
                PhoneNumber = dto.PhoneNumber,
                StaffId = staff.Id
            };
            mContext.Students.Add(student);

            signUpRequest.IsApproved = true;

            mContext.SaveChanges();
            return Ok(ErrorConstant.Approved);
        }

    }
}
