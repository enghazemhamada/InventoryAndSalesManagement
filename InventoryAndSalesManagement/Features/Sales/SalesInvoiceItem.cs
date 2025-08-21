using InventoryAndSalesManagement.Features.Products;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryAndSalesManagement.Features.Sales
{
    public class SalesInvoiceItem
    {
        public int Id { get; set; }

        [ForeignKey("SalesInvoice")]
        public int SalesInvoiceId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Precision(10, 2)]
        [Range(0.01, 999999999)]
        public decimal Price { get; set; }

        public Product Product { get; set; }
        public SalesInvoice SalesInvoice { get; set; }
    }
}
