using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService, SignInManager<ApplicationUser> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }

        #region Login

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {

            if (!ModelState.IsValid) return View(viewModel);

            var user = _accountService.ValidateUser(viewModel);

            if (user == null)
            {
                ModelState.AddModelError("InvalidLogin", "Inalid Email or Password");
                return View(viewModel);
            }

            var result = _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, false).Result;

            if (result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your Acount Is Not Allowed");

            if (result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your Acount Is Locked Out");

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View(viewModel);
        }

        #endregion

        #region Logout

        [HttpPost]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region AccessDenied

        public IActionResult AccessDenied()
        {
            return View();  
        }

        #endregion

    }
}
