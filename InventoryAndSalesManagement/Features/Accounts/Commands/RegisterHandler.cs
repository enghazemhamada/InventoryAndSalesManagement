using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InventoryAndSalesManagement.Features.Accounts.Commands
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, IdentityResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser appUser = new ApplicationUser
            {
                Name = request.userViewModel.Name,
                Email = request.userViewModel.Email,
                UserName = request.userViewModel.Email
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, request.userViewModel.Password);
            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(appUser, isPersistent: false);
            }
            return result;
        }
    }
}
