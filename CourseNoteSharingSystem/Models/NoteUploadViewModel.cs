namespace CourseNoteSharingSystem.Models
{
    public class NoteUploadViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public IFormFile File { get; set; } // Gerçek dosya buraya gelir
    }
}
