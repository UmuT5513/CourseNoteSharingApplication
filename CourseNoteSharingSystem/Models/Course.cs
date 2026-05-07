namespace CourseNoteSharingSystem.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Department { get; set; }

        public string Description { get; set; }


        // Relationship: The Foreign Key
        public int? UserId { get; set; } // nullable, çünkü kullanıcı silinse de kurs sistemde kalabilir.

        public ICollection<Note> Notes { get; set; } = new List<Note>();
        public User? User { get; set; }
        
    }
}
