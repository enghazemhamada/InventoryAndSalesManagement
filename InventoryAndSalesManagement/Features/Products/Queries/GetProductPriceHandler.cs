using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;

namespace InventoryAndSalesManagement.Features.Products.Queries
{
    public class GetProductPriceHandler : IRequestHandler<GetProductPriceQuery, ProductPriceViewModel>
    {
        private readonly ApplicationDbContext _context;

        public GetProductPriceHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductPriceViewModel> Handle(GetProductPriceQuery request, CancellationToken cancellationToken)
        {
            Product product = await _context.Products.FindAsync(request.id);
            if(product != null)
            {
                ProductPriceViewModel productPriceVM = new ProductPriceViewModel
                {
                    Price = product.Price,
                    Stock = product.QuantityInStock
                };
                return productPriceVM;
            }
            return null;
        }
    }
}
