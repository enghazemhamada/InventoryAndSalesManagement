using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAndSalesManagement.Features.Sales.Queries
{
    public class GetSalesInvoiceAddDataHandler : IRequestHandler<GetSalesInvoiceAddDataQuery, SalesInvoiceWithCustomersWithProductsViewModel>
    {
        private readonly ApplicationDbContext _context;

        public GetSalesInvoiceAddDataHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SalesInvoiceWithCustomersWithProductsViewModel> Handle(GetSalesInvoiceAddDataQuery request, CancellationToken cancellationToken)
        {
            SalesInvoiceWithCustomersWithProductsViewModel salesInvoiceWithCustomersWithProductsVM
                = new SalesInvoiceWithCustomersWithProductsViewModel
                {
                    Customers = await _context.Customers.ToListAsync(),
                    Products = await _context.Products.ToListAsync()
                };

            return salesInvoiceWithCustomersWithProductsVM;
        }
    }
}
