namespace CourseNoteSharingSystem.Models
{
    public class DownloadLog
    {
        public int Id { get; set; }

        public DateTime DownloadDate { get; set; }

        // USER
        public int UserId { get; set; }
        public User User { get; set; }

        // NOTE
        public int NoteId { get; set; }
        public Note Note { get; set; }
    }
}
