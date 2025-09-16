using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;

namespace InventoryAndSalesManagement.Features.Customers.Queries
{
    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly ApplicationDbContext _context;

        public GetCustomerByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers.FindAsync(request.id);
        }
    }
}
