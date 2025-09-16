using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAndSalesManagement.Features.Products.Queries
{
    public class SearchProductsHandler : IRequestHandler<SearchProductsQuery, List<Product>>
    {
        private readonly ApplicationDbContext _context;

        public SearchProductsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            List<Product> products = await _context.Products.ToListAsync();
            if(!string.IsNullOrWhiteSpace(request.search))
            {
                products = products.Where(p => p.Name.Contains(request.search)).ToList();
            }
            return products;
        }
    }
}
