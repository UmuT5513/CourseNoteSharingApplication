namespace CourseNoteSharingSystem.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string CourseName { get; set; }

        public string CourseCode { get; set; }

        public string Department { get; set; }

        public string Description { get; set; }


        // Relationship: The Foreign Key
        public int? UserId { get; set; } // nullable, çünkü kullanıcı silinse de kurs sistemde kalabilir.
        public User? User { get; set; }

        public ICollection<Note> Notes { get; set; } = new List<Note>();
        
        
    }
}
