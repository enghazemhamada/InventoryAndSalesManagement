using InventoryAndSalesManagement.Features.Customers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryAndSalesManagement.Features.Sales
{
    public class SalesInvoice
    {
        public int Id { get; set; }
        public int InvoiceNo { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }

        [Precision(10, 2)]
        public decimal Total { get; set; }

        public Customer Customer { get; set; }
        public List<SalesInvoiceItem> Items { get; set; }
    }
}
