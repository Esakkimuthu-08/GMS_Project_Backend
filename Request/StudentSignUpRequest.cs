namespace Grievance_Management_System.Request
{
    public class StudentSignUpRequest
    {
        public string RollNo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string StaffCode { get; set; }
    }
}
