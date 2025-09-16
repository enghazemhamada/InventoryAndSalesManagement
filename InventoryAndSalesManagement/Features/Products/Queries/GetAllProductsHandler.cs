using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAndSalesManagement.Features.Products.Queries
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllProductsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products.ToListAsync();
        }
    }
}
