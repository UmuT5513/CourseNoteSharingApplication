using CourseNoteSharingSystem.Data;
using CourseNoteSharingSystem.Models;
using CourseNoteSharingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseNoteSharingSystem.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly CourseNoteSharingSystemContext _context;
        private readonly UserManager<User> _userManager;

        public ProfileController(CourseNoteSharingSystemContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var uploadedNotes = await _context.Note
                .Include(n => n.Course)
                .Where(n => n.UserId == user.Id)
                .OrderByDescending(n => n.UploadDate)
                .ToListAsync();

            var downloadHistory = await _context.DownloadLogs
                .Include(d => d.Note)
                .Where(d => d.UserId == user.Id)
                .OrderByDescending(d => d.DownloadDate)
                .Take(10)
                .ToListAsync();

            var model = new ProfileViewModel
            {
                User = user,
                UploadedNotes = uploadedNotes,
                DownloadHistory = downloadHistory,
                TotalDownloads = uploadedNotes.Sum(n => n.DownloadCount),
                TotalComments = await _context.Comments.CountAsync(c => c.UserId == user.Id)
            };

            return View(model);
        }
    }
}
