using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAndSalesManagement.Features.Sales.Queries
{
    public class GetSalesInvoiceDeleteDataHandler : IRequestHandler<GetSalesInvoiceDeleteDataQuery, SalesInvoice>
    {
        private readonly ApplicationDbContext _context;

        public GetSalesInvoiceDeleteDataHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SalesInvoice> Handle(GetSalesInvoiceDeleteDataQuery request, CancellationToken cancellationToken)
        {
            return await _context.SalesInvoices.Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == request.id);
        }
    }
}
