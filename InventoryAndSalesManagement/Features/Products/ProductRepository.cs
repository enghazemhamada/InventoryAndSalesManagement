using InventoryAndSalesManagement.Infrastructure.Data;
using InventoryAndSalesManagement.Infrastructure.Repositories;

namespace InventoryAndSalesManagement.Features.Products
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }
    }
}
