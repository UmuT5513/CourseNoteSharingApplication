using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseNoteSharingSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CourseNoteSharingSystem.Data
{
    public class CourseNoteSharingSystemContext : IdentityDbContext<User, Role, int>
    {
        public CourseNoteSharingSystemContext (DbContextOptions<CourseNoteSharingSystemContext> options)
            : base(options)
        {
        }

        public DbSet<CourseNoteSharingSystem.Models.Note> Note { get; set; } = default!;
        public DbSet<CourseNoteSharingSystem.Models.Course> Course { get; set; } = default!;
        public DbSet<CourseNoteSharingSystem.Models.User> User { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Note>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Course>()
                .HasOne(c => c.User)
                .WithMany(u => u.Courses)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
