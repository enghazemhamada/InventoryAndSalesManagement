using Microsoft.AspNetCore.Identity;

namespace InventoryAndSalesManagement.Features.Accounts
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
