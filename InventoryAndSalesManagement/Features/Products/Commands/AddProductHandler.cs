using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;

namespace InventoryAndSalesManagement.Features.Products.Commands
{
    public class AddProductHandler : IRequestHandler<AddProductCommand, Product>
    {
        private readonly ApplicationDbContext _context;

        public AddProductHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new Product
            {
                Name = request.productVM.Name,
                Price = request.productVM.Price,
                QuantityInStock = request.productVM.QuantityInStock
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
