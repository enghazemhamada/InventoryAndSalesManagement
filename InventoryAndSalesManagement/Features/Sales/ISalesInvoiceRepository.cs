using InventoryAndSalesManagement.Infrastructure.Repositories;

namespace InventoryAndSalesManagement.Features.Sales
{
    public interface ISalesInvoiceRepository : IGenericRepository<SalesInvoice>
    {
        Task<List<SalesInvoice>> GetAllSalesInvoicesWithCustomerAsync();
        Task<SalesInvoice> GetSalesInvoiceWithCustomerAsync(int id);
        Task<SalesInvoice> GetSalesInvoiceWithItemsAsync(int id);
        IQueryable<SalesInvoice> GetSalesInvoicesWithItemsWithCustomers();
    }
}
