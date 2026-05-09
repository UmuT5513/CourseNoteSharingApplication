using CourseNoteSharingSystem.Data;
using CourseNoteSharingSystem.Models;
using CourseNoteSharingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseNoteSharingSystem.Controllers
{
    [Authorize(Roles = "User")]
    public class UserDashboardController : Controller
    {
        private readonly CourseNoteSharingSystemContext _context;
        private readonly UserManager<User> _userManager;


        public UserDashboardController(
            CourseNoteSharingSystemContext context,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var myNotes = await _context.Note
            .Include(n => n.Course)
            .Where(n => n.UserId == user.Id)
            .ToListAsync();

            var totalDownloads = myNotes.Sum(n => n.DownloadCount);

            var recentDownloads = await _context.DownloadLogs
            .Include(d => d.Note)
            .Where(d => d.UserId == user.Id)
            .OrderByDescending(d => d.DownloadDate)
            .Take(5)
            .ToListAsync();

            var myComments = await _context.Comments
            .Include(c => c.Note)
            .Where(c => c.UserId == user.Id)
            .OrderByDescending(c => c.CreatedAt)
            .Take(5)
            .ToListAsync();

            var vm = new UserDashboardViewModel
            {

                TotalNotes = myNotes.Count(),
                TotalDownloads = totalDownloads,
                TotalComments = myComments.Count(),
                ApprovedNotes = myNotes.Count(n => n.Status == NoteStatus.Approved),

                MyComments = myComments,
                MyNotes = myNotes,
                RecentDownloads = recentDownloads,
            };


            return View(vm);
        }
    }
}
