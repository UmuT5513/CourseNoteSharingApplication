using CourseNoteSharingSystem.Data;
using CourseNoteSharingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseNoteSharingSystem.Controllers
{
    public class NotesController : Controller
    {
        private readonly CourseNoteSharingSystemContext _context;
        private readonly UserManager<User> _userManager;

        public NotesController(CourseNoteSharingSystemContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Notes
        public async Task<IActionResult> Index(
            string search,
            int? courseId,
            string sortOrder)
        {
            var notes = _context.Note
            .Include(n => n.Course)
            .Include(n => n.User)
            .AsQueryable();

            // SEARCH
            if (!string.IsNullOrEmpty(search))
            {
                notes = notes.Where(n =>
                    n.Title.Contains(search) || 
                    n.Description.Contains(search)
                );
            }

            // COURSE FILTER
            if (courseId.HasValue)
            {
                notes = notes.Where(n =>
                    n.CourseId == courseId.Value);
            }

            // SORT
            notes = sortOrder switch
            {
                "downloads" => notes.OrderByDescending(n => n.DownloadCount),

                "oldest" => notes.OrderBy(n => n.UploadDate),

                _ => notes.OrderByDescending(n => n.UploadDate)
            };

            // Dropdown için
            ViewBag.Courses = new SelectList(
                _context.Course,
                "Id",
                "CourseName");

            return View(await notes.ToListAsync());
        }


        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .Include(n => n.Course)
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            // Dropdown için course listesi
            ViewBag.Courses = new SelectList(_context.Course.ToList(), "Id", "Name");
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteUploadViewModel model)
        {
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".txt", ".pptx" };
            var fileExtension = Path.GetExtension(model.File.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("File", "Only PDF, Word, PowerPoint and TXT files can be uploaded.");
                return View(model);
            }

            if (model.File.Length > 52428800)
            {
                ModelState.AddModelError("File", "File size cannot exceed 50MB.");
                return View(model);
            }

            if (ModelState.IsValid && model.File != null) { 
                // Dosyayı sunucuya kaydet
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder); // klasör yoksa oluştur

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }


                var currentUser = await _userManager.GetUserAsync(User);

                // Note entity'sini kaydet
                var note = new Note
                {
                    Title = model.Title,
                    Description = model.Description,
                    CourseId = model.CourseId,
                    FilePath = "/uploads/" + uniqueFileName, // DB'ye bu path kaydedilir
                    UploadDate = DateTime.Now,
                    UserId = currentUser.Id, // notu yükleyen kullanıcı
                    Status = NoteStatus.Pending // Yeni yüklenen notlar onaylanmay bekler

                };
            
                _context.Add(note);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            

            return View(model);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,FilePath,UploadDate")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Note.FindAsync(id);
            if (note != null)
            {
                _context.Note.Remove(note);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }


        
        public async Task<IActionResult> Download(int id)
        {
            var note = await _context.Note
                .FirstOrDefaultAsync(n => n.Id == id);

            if (note == null)
                return NotFound();

            // DOWNLOAD COUNT
            note.DownloadCount++;

            // CURRENT USER
            var user = await _userManager.GetUserAsync(User);

            // LOG
            var log = new DownloadLog
            {
                DownloadDate = DateTime.Now,
                NoteId = note.Id,
                UserId = user.Id
            };

            _context.DownloadLogs.Add(log);

            await _context.SaveChangesAsync();

            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                note.FilePath.TrimStart('/'));

            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            var fileName = Path.GetFileName(fullPath);

            return PhysicalFile(
                fullPath,
                "application/octet-stream",
                fileName);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult PendingNotes()
        {
            var notes = _context.Note.Where(n => n.Status == NoteStatus.Pending).ToList();
            return View(notes);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var note = await _context.Note.FirstOrDefaultAsync(n => n.Id == id);
            if (note == null) return NotFound();
            note.Status = NoteStatus.Approved;
            _context.Update(note);
            await _context.SaveChangesAsync();
            return RedirectToAction("PendingNotes");
        }
    }
}
