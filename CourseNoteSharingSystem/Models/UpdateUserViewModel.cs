namespace CourseNoteSharingSystem.Models
{
    public class UpdateUserViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateOnly BirthDate { get; set; }
        public List<RoleViewModel> Roles { get; set; }

    }

    public class RoleViewModel
    {
        public string RoleName { get; set; }

        public bool IsSelected { get; set; }
    }
}
