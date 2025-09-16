using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAndSalesManagement.Features.Sales.Queries
{
    public class GetAllSalesInvoicesHandler : IRequestHandler<GetAllSalesInvoicesQuery, List<SalesInvoice>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllSalesInvoicesHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SalesInvoice>> Handle(GetAllSalesInvoicesQuery request, CancellationToken cancellationToken)
        {
            return await _context.SalesInvoices.Include(x => x.Customer).ToListAsync();
        }
    }
}
