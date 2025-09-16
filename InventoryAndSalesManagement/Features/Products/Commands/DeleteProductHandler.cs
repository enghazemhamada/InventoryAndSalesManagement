using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;

namespace InventoryAndSalesManagement.Features.Products.Commands
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeleteProductHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            Product productFromDB = await _context.Products.FindAsync(request.id);
            if(productFromDB != null)
            {
                _context.Products.Remove(productFromDB);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
