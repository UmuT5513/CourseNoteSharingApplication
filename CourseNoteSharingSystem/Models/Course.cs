namespace CourseNoteSharingSystem.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }


        // Relationship: The Foreign Key
        public int? UserId { get; set; }

        // Navigation Property: Allows you to access User data from a Note
        public User? User { get; set; }
    }
}
