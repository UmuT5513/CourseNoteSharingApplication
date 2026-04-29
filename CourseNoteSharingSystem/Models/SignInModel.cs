using System.ComponentModel.DataAnnotations;

namespace CourseNoteSharingSystem.Models
{
    
    public class SignInModel
    {
        [Required(ErrorMessage ="Username must be entered!")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Password must be entered!")]
        public string Password { get; set; }
    }
}
