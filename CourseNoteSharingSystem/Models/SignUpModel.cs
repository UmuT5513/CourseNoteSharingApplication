namespace CourseNoteSharingSystem.Models
{
    public class SignUpModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    
        public string Email { get; set; }
    
        public int birthDate { get; set; }

        public bool isAdmin { get; set; }
    }
}
