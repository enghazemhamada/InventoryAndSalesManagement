using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InventoryAndSalesManagement.Features.Sales
{
    public class SalesInvoiceItemViewModel
    {
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Precision(10, 2)]
        [Range(0.01, 999999999)]
        public decimal Price { get; set; }
    }
}
