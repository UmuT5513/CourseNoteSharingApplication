namespace CourseNoteSharingSystem.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }


        // Relationship: The Foreign Key
        public int UserId { get; set; }

        // Navigation Property: Allows you to access User data from a Note
        public User? User { get; set; }
    }
}
