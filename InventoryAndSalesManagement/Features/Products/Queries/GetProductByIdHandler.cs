using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;

namespace InventoryAndSalesManagement.Features.Products.Queries
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly ApplicationDbContext _context;

        public GetProductByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products.FindAsync(request.id);
        }
    }
}
