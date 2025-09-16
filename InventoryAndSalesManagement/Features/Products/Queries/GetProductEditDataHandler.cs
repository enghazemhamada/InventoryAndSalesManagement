using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;

namespace InventoryAndSalesManagement.Features.Products.Queries
{
    public class GetProductEditDataHandler : IRequestHandler<GetProductEditDataQuery, EditProductViewModel>
    {
        private readonly ApplicationDbContext _context;

        public GetProductEditDataHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EditProductViewModel> Handle(GetProductEditDataQuery request, CancellationToken cancellationToken)
        {
            Product productFromDB = await _context.Products.FindAsync(request.id);
            if(productFromDB != null)
            {
                EditProductViewModel productVM = new EditProductViewModel
                {
                    Id = productFromDB.Id,
                    Name = productFromDB.Name,
                    Price = productFromDB.Price,
                    QuantityInStock = productFromDB.QuantityInStock
                };
                return productVM;
            }
            return null;
        }
    }
}
