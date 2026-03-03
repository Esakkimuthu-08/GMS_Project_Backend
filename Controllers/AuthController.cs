using Azure.Core;
using Grievance_Management_System.AppDbContext;
using Grievance_Management_System.Auth;
using Grievance_Management_System.Constants;
using Grievance_Management_System.Model;
using Grievance_Management_System.Model.Auth;
using Grievance_Management_System.Request;
using Grievance_Management_System.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Grievance_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(GrievenceDbContext mContext, TokenService _token) : ControllerBase
    {

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email)
                || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(ErrorConstant.BadRequest);
            }

            User user = mContext.Users.Where(user => user.Email == loginRequest.Email).FirstOrDefault();

            if(user == null)
            {
                return Unauthorized(ErrorConstant.Unauthorized);
            }

            if(!user.IsActive)
            {
                return BadRequest(ErrorConstant.AccountDisabled);
            }

            if(user.PasswordHash != loginRequest.Password)
            {
                return Unauthorized(ErrorConstant.Unauthorized);
            }
            var token = _token.CreateToken(user);
            return Ok(new { token });


        }

        [HttpPost("StaffSignUp")]
        public IActionResult StaffSignUp([FromBody] StaffSignUpRequest request)
        {
            if (string.IsNullOrEmpty(request.Name)
                || string.IsNullOrEmpty(request.Email)
                || string.IsNullOrEmpty(request.PasswordHash))
            {
                return BadRequest(ErrorConstant.BadRequest);
            }

            User user = mContext.Users.FirstOrDefault(user => user.Email == request.Email);

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

        
        [HttpGet("getAllStaffSignUpRequest")]

        public IActionResult GetAllStaffSignUpRequest()
        {
            return Ok(mContext.StaffSignUp.ToList());
        }
        
        [HttpPost("StudentSignUp")]

        public IActionResult StudentSignUp([FromBody] StudentSignUpRequest studentSignUpRequest)
        {
            if (string.IsNullOrEmpty(studentSignUpRequest.Name) ||
                string.IsNullOrEmpty(studentSignUpRequest.Email) ||
                string.IsNullOrEmpty(studentSignUpRequest.PasswordHash) ||
                string.IsNullOrEmpty(studentSignUpRequest.StaffCode)
                )
            {
                return BadRequest(ErrorConstant.BadRequest);
            }

            User user = mContext.Users.Where(user => user.Email == studentSignUpRequest.Email).FirstOrDefault();

            if (user != null)
            {
                if (user.IsActive == false)
                    return BadRequest(ErrorConstant.AccountDisabled);

                return BadRequest(ErrorConstant.AccountExist);
            }

            bool PendingRequest = mContext.StudentSignUp.Any(request => request.Email == studentSignUpRequest.Email && request.IsApproved == false);

            if (PendingRequest)
            {
                return BadRequest(ErrorConstant.AccountPending);
            }

            Staff staffCode = mContext.Staffs.Where(code => code.StaffCode == studentSignUpRequest.StaffCode).FirstOrDefault();

            if (staffCode == null)
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
       
        [HttpGet("getAllUser")]

        public IActionResult GetAllUser()
        {
            return Ok(mContext.Users.ToList());
        }
    }
}
