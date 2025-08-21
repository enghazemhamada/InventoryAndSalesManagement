using System.ComponentModel.DataAnnotations;

namespace InventoryAndSalesManagement.Features.Customers
{
    public class AddCustomerViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; }
    }
}
