namespace CourseNoteSharingSystem.ViewModels
{
    public class SignUpModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    
        public string Email { get; set; }
    
        public DateOnly birthDate { get; set; }

        public bool isAdmin { get; set; }

        public bool deneme { get; set; } = false;
    }
}
