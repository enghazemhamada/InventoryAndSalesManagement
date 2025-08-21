using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InventoryAndSalesManagement.Features.Products
{
    public class AddProductViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [Precision(10, 2)]
        [Range(0.01, 999999999)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Quantity In Stock")]
        public int QuantityInStock { get; set; }
    }
}
