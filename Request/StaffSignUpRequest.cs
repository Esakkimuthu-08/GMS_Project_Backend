namespace Grievance_Management_System.Request
{
    public class StaffSignUpRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
