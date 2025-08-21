using InventoryAndSalesManagement.Infrastructure.Data;
using InventoryAndSalesManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InventoryAndSalesManagement.Features.Sales
{
    public class SalesInvoiceRepository : GenericRepository<SalesInvoice>, ISalesInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public SalesInvoiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SalesInvoice>> GetAllSalesInvoicesWithCustomerAsync()
        {
            return await _context.SalesInvoices.Include(x => x.Customer).ToListAsync();
        }

        public async Task<SalesInvoice> GetSalesInvoiceWithCustomerAsync(int id)
        {
            return await _context.SalesInvoices.Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<SalesInvoice> GetSalesInvoiceWithItemsAsync(int id)
        {
            return await _context.SalesInvoices.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<SalesInvoice> GetSalesInvoicesWithItemsWithCustomers()
        {
            return _context.SalesInvoices.Include(i => i.Customer).Include(i => i.Items).ThenInclude(it => it.Product).AsQueryable();
        }
    }
}
