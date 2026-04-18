using Microsoft.AspNetCore.Identity;

namespace CourseNoteSharingSystem.Models
{
    public class Role : IdentityRole<int>
    {
        public bool isUpdated { get; set; }
    }
}
