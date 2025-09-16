using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAndSalesManagement.Features.Customers.Queries
{
    public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, List<Customer>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllCustomersHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers.ToListAsync();
        }
    }
}
