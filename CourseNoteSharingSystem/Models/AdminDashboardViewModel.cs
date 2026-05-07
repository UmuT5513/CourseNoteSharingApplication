using Microsoft.AspNetCore.Identity;

namespace CourseNoteSharingSystem.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalRoles { get; set; }
        public int TotalProducts { get; set; }

        // Yeni Eklenenler
        public int TotalCourses { get; set; }
        public int TotalNotes { get; set; }

        public List<User> RecentUsers { get; set; }
    }
}
