using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;

namespace InventoryAndSalesManagement.Features.Products.Commands
{
    public class EditProductHandler : IRequestHandler<EditProductCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public EditProductHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            Product productFromDB = await _context.Products.FindAsync(request.id);
            if(productFromDB != null)
            {
                productFromDB.Name = request.productVM.Name;
                productFromDB.Price = request.productVM.Price;
                productFromDB.QuantityInStock = request.productVM.QuantityInStock;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
