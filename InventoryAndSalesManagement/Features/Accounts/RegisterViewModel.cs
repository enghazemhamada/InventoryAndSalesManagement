using System.ComponentModel.DataAnnotations;

namespace InventoryAndSalesManagement.Features.Accounts
{
    public class RegisterViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
