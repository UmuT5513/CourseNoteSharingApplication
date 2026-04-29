using CourseNoteSharingSystem.Models;
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
                var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent:false, lockoutOnFailure:true);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username or password is incorrect.");
                }
            }

            return View(model);
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
