using CourseNoteSharingSystem.Models;

namespace CourseNoteSharingSystem.ViewModels
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<Note> UploadedNotes { get; set; } = new();
        public List<DownloadLog> DownloadHistory { get; set; } = new();
        public int TotalDownloads { get; set; }
        public int TotalComments { get; set; }
    }
}
