using CourseNoteSharingSystem.Migrations;

namespace CourseNoteSharingSystem.ViewModels
{
    public class CourseCreateViewModel
    {
        public int Id { get; set; }

        public string CourseName { get; set; }

        public string CourseCode { get; set; }

        public string Department { get; set; }

        public string Description { get; set; }


    }
}
