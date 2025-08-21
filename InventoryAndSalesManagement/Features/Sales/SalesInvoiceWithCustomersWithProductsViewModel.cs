using InventoryAndSalesManagement.Features.Customers;
using InventoryAndSalesManagement.Features.Products;

namespace InventoryAndSalesManagement.Features.Sales
{
    public class SalesInvoiceWithCustomersWithProductsViewModel
    {
        public int CustomerId { get; set; }
        public List<SalesInvoiceItemViewModel> Items { get; set; }

        public IEnumerable<Customer>? Customers { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}
