using CourseNoteSharingSystem.Models;

namespace CourseNoteSharingSystem.ViewModels
{
    public class UserDashboardViewModel
    {
        public int TotalNotes { get; set; }

        public int TotalDownloads { get; set; }

        public int TotalComments { get; set; }

        public int ApprovedNotes { get; set; }

        public List<Note> MyNotes { get; set; }

        public List<DownloadLog> RecentDownloads { get; set; }

        public List<Comment> MyComments { get; set; }
    }
}
