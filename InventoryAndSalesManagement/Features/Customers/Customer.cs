using InventoryAndSalesManagement.Features.Sales;
using System.ComponentModel.DataAnnotations;

namespace InventoryAndSalesManagement.Features.Customers
{
    public class Customer
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; }

        public List<SalesInvoice> SalesInvoices { get; set; }
    }
}
