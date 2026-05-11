namespace CourseNoteSharingSystem.ViewModels
{
    public class UpdateProfileViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }

        public string? Bio { get; set; }

        public string? LinkedInProfileLink { get; set; }

        public string? OgrenciMail { get; set; }

        public string? OgrenciNumarasi { get; set; }

        public DateOnly birthDate { get; set; }
    
        public string? PhoneNumber { get; set; }
    }
}
