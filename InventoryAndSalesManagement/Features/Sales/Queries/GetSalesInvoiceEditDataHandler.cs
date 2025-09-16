using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAndSalesManagement.Features.Sales.Queries
{
    public class GetSalesInvoiceEditDataHandler : IRequestHandler<GetSalesInvoiceEditDataQuery, EditSalesInvoiceWithCustomersWithProductsViewModel>
    {
        private readonly ApplicationDbContext _context;

        public GetSalesInvoiceEditDataHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EditSalesInvoiceWithCustomersWithProductsViewModel> Handle(GetSalesInvoiceEditDataQuery request, CancellationToken cancellationToken)
        {
            SalesInvoice salesInvoice = await _context.SalesInvoices.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == request.id);
            if(salesInvoice != null)
            {
                EditSalesInvoiceWithCustomersWithProductsViewModel salesInvoiceWithCustomersWithProductsVM =
                    new EditSalesInvoiceWithCustomersWithProductsViewModel
                    {
                        Id = salesInvoice.Id,
                        CustomerId = salesInvoice.CustomerId,
                        Items = salesInvoice.Items.Select(i => new SalesInvoiceItemViewModel
                        {
                            ProductId = i.ProductId,
                            Quantity = i.Quantity,
                            Price = i.Price
                        }).ToList(),
                        Customers = await _context.Customers.ToListAsync(),
                        Products = await _context.Products.ToListAsync()
                    };

                return salesInvoiceWithCustomersWithProductsVM;
            }
            return null;
        }
    }
}
