using InventoryAndSalesManagement.Features.Products;
using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAndSalesManagement.Features.Sales.Commands
{
    public class DeleteSalesInvoiceHandler : IRequestHandler<DeleteSalesInvoiceCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeleteSalesInvoiceHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteSalesInvoiceCommand request, CancellationToken cancellationToken)
        {
            SalesInvoice salesInvoice = await _context.SalesInvoices.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == request.id);
            if(salesInvoice != null)
            {
                foreach(var item in salesInvoice.Items)
                {
                    Product product = await _context.Products.FindAsync(item.ProductId);
                    if(product != null)
                        product.QuantityInStock += item.Quantity;
                }

                _context.SalesInvoices.Remove(salesInvoice);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
