using Grievance_Management_System.Enum;

namespace Grievance_Management_System.Request
{
    public class ApproveStaffDto
    {
        public int SignupRequestId { get; set; }
        public RoleEnum Role { get; set; }
        public string StaffCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
