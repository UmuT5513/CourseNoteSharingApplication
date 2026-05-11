using CourseNoteSharingSystem.Models;
using CourseNoteSharingSystem.Data;
using CourseNoteSharingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CourseNoteSharingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {

        private readonly CourseNoteSharingSystemContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;


        // user ve role ler için managers, diğer sınıflar için context kullanacağız
        public AdminDashboardController(
            CourseNoteSharingSystemContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = _userManager.Users.Count(),
                TotalRoles = _roleManager.Roles.Count(),
                

                // Yeni Eklenen Veritabanı Sorguları
                TotalCourses = _context.Course.Count(),
                TotalNotes = _context.Note.Count(),

                RecentUsers = _userManager.Users.OrderByDescending(u => u.Id).Take(5).ToList()
            };

            return View(model);
        }


        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public IActionResult Roles()
        {
            List<Role> roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult Courses()
        {
            var courses = _context.Course.ToList();
            return View(courses);
        }


        // update user GET method
          
        public async Task<IActionResult> UpdateUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();

            var model = new UpdateUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                BirthDate = user.birthDate,
                Roles = allRoles.Select(r => new RoleViewModel
                {
                    RoleName = r.Name,
                    IsSelected = userRoles.Contains(r.Name)
                }).ToList()
            };

            return View(model);
        }

        // update user POST method
        [HttpPost]
        
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null) return NotFound();

            user.UserName = model.UserName;
            user.Email = model.Email;

            // post view i oluşturulacağı zaman her rol için bir checkbox eklerizz, o checkbox da seçili olan roller bizim rollere eklenir
            var userRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName).ToList();

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {

                ModelState.AddModelError("", "The user could not be updated.");

                return View(model);
            }

            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, selectedRoles);

            return RedirectToAction("Users");
        }


        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("The user could not be deleted.");
            }
            return RedirectToAction("Users");
        }


        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError("", "Role name cannot be empty.");
                return View();
            }
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                ModelState.AddModelError("", "Role already exists.");
                return View();
            }
            var result = await _roleManager.CreateAsync(new Role { Name = roleName, isUpdated = false });
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "The role could not be created.");
                return View();
            }
            return RedirectToAction("Roles");
        }

        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return NotFound();
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest("The role could not be deleted.");
            }
            return RedirectToAction("Roles");
        }

        public async Task<IActionResult> UpdateRole(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return NotFound();

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(int Id, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(Id.ToString());
            if (role == null) return NotFound();

            role.Name = roleName;
            role.isUpdated = true;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Roles");

            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(role);
        }


        public async Task<IActionResult> Notes(NoteStatus? status)
        {
            if (status == NoteStatus.Pending)
            {
                var pendingNotes = await _context.Note
                .Include(n => n.Course)
                .Include(n => n.User)
                .Where(n => n.Status == NoteStatus.Pending)
                .OrderByDescending(n => n.UploadDate)
                .ToListAsync();
                return View(pendingNotes);
            }
            else if (status == NoteStatus.Rejected)
            {
                var rejectedNotes = await _context.Note
                .Include(n => n.Course)
                .Include(n => n.User)
                .Where(n => n.Status == NoteStatus.Rejected)
                .OrderByDescending(n => n.UploadDate)
                .ToListAsync();
                return View(rejectedNotes);
            }
            else if (status == NoteStatus.Approved)
            {
                var approvedNotes = await _context.Note
                .Include(n => n.Course)
                .Include(n => n.User)
                .Where(n => n.Status == NoteStatus.Approved)
                .OrderByDescending(n => n.UploadDate)
                .ToListAsync();
                return View(approvedNotes);
            }
            else { 
            var notes = await _context.Note
            .Include(n => n.Course)
            .Include(n => n.User)
            .OrderByDescending(n => n.UploadDate)
            .ToListAsync();

            return View(notes);
            } 
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var note = await _context.Note.FirstOrDefaultAsync(n => n.Id == id);
            if (note == null) return NotFound();
            note.Status = NoteStatus.Approved;
            _context.Update(note);
            await _context.SaveChangesAsync();
            return RedirectToAction("PendingNotes");
        }


        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var note = await _context.Note.FirstOrDefaultAsync(n => n.Id == id);
            if (note == null) return NotFound();
            note.Status = NoteStatus.Rejected;
            _context.Update(note);
            await _context.SaveChangesAsync();
            return RedirectToAction("PendingNotes");
        }

        public async Task<IActionResult> PendingNotes()
        {
            var notes = _context.Note.Where(n => n.Status == NoteStatus.Pending).ToList();
            return View(notes);
        }




    }
}
