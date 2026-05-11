using CourseNoteSharingSystem.Models;

namespace CourseNoteSharingSystem.ViewModels
{
    public class UserMyCommentsModelView
    {
        public Note NoteToComment { get; set; } // hangi nota yorum yaptığı
        public string content { get; set; } // yorumun içeriği
        public DateTime DateOfComment { get; set; } // yorumun yapıldığı tarih

    }
}
