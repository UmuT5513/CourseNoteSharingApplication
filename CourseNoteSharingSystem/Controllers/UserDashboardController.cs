using CourseNoteSharingSystem.Data;
using CourseNoteSharingSystem.Models;
using CourseNoteSharingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

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


        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var userId = _userManager.GetUserId(User);

            // 2. Kullanıcıyı veritabanından getirir
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            // 3. Veritabanı modelini, View'da kullanacağımız ViewModel'e eşler (Mapping)
            var model = new UpdateProfileViewModel
            {
                FullName = user.FullName,
                Bio = user.Bio,
                LinkedInProfileLink = user.LinkedInProfileLink,
                OgrenciMail = user.OgrenciMail,
                OgrenciNumarasi = user.OgrenciNumarasi,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            // 1. Model kurallarını (Required vb.) kontrol eder
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            // 2. Güncellenecek kullanıcıyı tekrar bulur
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.Bio = model.Bio;
            user.LinkedInProfileLink = model.LinkedInProfileLink;
            user.OgrenciMail = model.OgrenciMail;
            user.OgrenciNumarasi = model.OgrenciNumarasi;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Your profile has been updated successfully.";
                return RedirectToAction("Index"); // Dashboard ana sayfasına döner
            }

            // Hata varsa model hatalarına ekler
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }


        public async Task<IActionResult> SummaryProfile()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var model = new SummaryProfileViewModel
            {
                FullName = user.FullName,
                Bio = user.Bio,
                LinkedInProfileLink = user.LinkedInProfileLink,
                OgrenciMail = user.OgrenciMail,
                OgrenciNumarasi = user.OgrenciNumarasi,
                PhoneNumber = user.PhoneNumber
            };
            return View(model);
        }
    }
}
