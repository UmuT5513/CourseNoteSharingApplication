using Microsoft.AspNetCore.Identity;

namespace CourseNoteSharingSystem.Models
{
    public class User : IdentityUser<int>
    {
        public DateOnly birthDate { get; set; }

        // One user can have many notes
        public ICollection<Note> Notes { get; set; } = new List<Note>();

        // one user can create many notes in many courses
        public ICollection<Course> Courses { get; set; } = new List<Course>();

        public ICollection<DownloadLog> DownloadLogs { get; set; }
    }
}
