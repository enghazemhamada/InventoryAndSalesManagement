using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InventoryAndSalesManagement.Features.Accounts.Commands
{
    public class LoginHandler : IRequestHandler<LoginCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser appUser = await _userManager.FindByNameAsync(request.userViewModel.Email);
            if(appUser != null)
            {
                bool found = await _userManager.CheckPasswordAsync(appUser, request.userViewModel.Password);
                if(found)
                {
                    await _signInManager.SignInAsync(appUser, request.userViewModel.RememberMe);
                    return true;
                }
            }
            return false;
        }
    }
}
