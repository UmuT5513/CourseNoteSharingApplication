namespace CourseNoteSharingSystem.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        // NOTE RELATION
        public int NoteId { get; set; }
        public Note Note { get; set; }

        // USER RELATION
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
