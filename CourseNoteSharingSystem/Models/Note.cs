using System.ComponentModel;

namespace CourseNoteSharingSystem.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }

        public int DownloadCount { get; set; } = 0;

        public NoteStatus Status { get; set; }


        // Relationships: The Foreign Key
        public int? UserId { get; set; }

        public int CourseId { get; set; }


        // Navigation Property: Allows you to access User data from a Note
        public User? User { get; set; }
        public Course? Course { get; set; }

        
    }

    public enum NoteStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}


