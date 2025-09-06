using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAndSalesManagement.Features.Accounts
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("/Features/Accounts/Views/Register.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel userViewModel)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser
                {
                    Name = userViewModel.Name,
                    Email = userViewModel.Email,
                    UserName = userViewModel.Email,
                    PasswordHash = userViewModel.Password
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, userViewModel.Password);
                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(appUser, isPersistent: false);
                    return RedirectToAction("Index", "Product");
                }
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("/Features/Accounts/Views/Register.cshtml", userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LoginOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Register");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("/Features/Accounts/Views/Login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel userViewModel)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser appUser = await _userManager.FindByNameAsync(userViewModel.Email);
                if(appUser != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(appUser, userViewModel.Password);
                    if(found)
                    {
                        await _signInManager.SignInAsync(appUser, userViewModel.RememberMe);
                        return RedirectToAction("Index", "Product");
                    }
                }
                ModelState.AddModelError("", "Email Or Password Wrong!");
            }
            return View("/Features/Accounts/Views/Login.cshtml", userViewModel);
        }
    }
}
