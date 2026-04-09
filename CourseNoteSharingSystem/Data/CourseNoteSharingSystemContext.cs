using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseNoteSharingSystem.Models;

namespace CourseNoteSharingSystem.Data
{
    public class CourseNoteSharingSystemContext : DbContext
    {
        public CourseNoteSharingSystemContext (DbContextOptions<CourseNoteSharingSystemContext> options)
            : base(options)
        {
        }

        public DbSet<CourseNoteSharingSystem.Models.Note> Note { get; set; } = default!;
        public DbSet<CourseNoteSharingSystem.Models.Course> Course { get; set; } = default!;
        public DbSet<CourseNoteSharingSystem.Models.User> User { get; set; } = default!;
    }
}
