using Grievance_Management_System.Enum;

namespace Grievance_Management_System.Auth
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public RoleEnum Role { get; set; }
        public bool IsActive { get; set; }
    }
}
