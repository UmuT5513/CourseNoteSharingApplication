namespace CourseNoteSharingSystem.ViewModels
{
    public class UserWithRolesViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public string RoleName { get; set; }
        public DateOnly BirthDate { get; set; }
        public List<string> Roles { get; set; } 
    }
}
