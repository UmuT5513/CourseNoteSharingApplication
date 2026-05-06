using CourseNoteSharingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CourseNoteSharingSystem.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public HomeController(
            UserManager<User> userManager, 
            RoleManager<Role> roleManager, 
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult SignUp()
        {
            return View(new SignUpModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    birthDate = model.birthDate

                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.isAdmin)
                    {
                        if (!await _roleManager.RoleExistsAsync("Admin"))
                        {
                            await _roleManager.CreateAsync(new Role { Name = "Admin", isUpdated = false });
                        }
                        await _userManager.AddToRoleAsync(user, "Admin");
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        if (!await _roleManager.RoleExistsAsync("User"))
                        {
                            await _roleManager.CreateAsync(new Role { Name = "User", isUpdated = false });
                        }
                        await _userManager.AddToRoleAsync(user, "User");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        // formu oluşturacak olan endpoint. Bunun post halini yazmamız gerekiyor.
        public IActionResult SignIn()
        {
            return View(new SignInModel());
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            // parametrik olarak formdan dönen modeli karşılamamız gerekiyor.

            if (ModelState.IsValid)
            {
                // Sign in logic here
                var user = await _userManager.FindByNameAsync(model.UserName);
                var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent:model.RememberMe, lockoutOnFailure:true);

                if (signInResult.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("AdminDashboard");
                    }
                    else 
                    { 
                        return RedirectToAction("UserDashboard"); 
                    }
                    
                }
                else if (signInResult.IsLockedOut)
                {
                    var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                    var remainingLockoutTime = (lockoutEnd.Value.UtcDateTime - DateTime.UtcNow).Minutes;
                    ModelState.AddModelError(string.Empty, $"Your account is locked out. Please try again in {remainingLockoutTime} minutes.");
                }


                // giriş yapan kullanıcının kaç hatalı giriş yaptığı
                var message = string.Empty;

                if (user != null)
                {
                    var failedLogins = await _userManager.GetAccessFailedCountAsync(user);
                    message = $"{(_userManager.Options.Lockout.MaxFailedAccessAttempts - failedLogins)} more attempts remaining.";
                }
                else
                {
                    message = "Username or password is incorrect.";
                }
                ModelState.AddModelError(string.Empty, message);
            }

            return View(model);
        }


        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }


        // Authrization işlemleri.

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            return View();
        }


        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserDashboard()
        {
            var users = _userManager.Users.ToList();
            var userViewModels = new List<UserWithRolesViewModel>();

            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserWithRolesViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    BirthDate = user.birthDate,
                    Roles = roles.ToList()
                });
            }
            return View(userViewModels);
        }   
        
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
