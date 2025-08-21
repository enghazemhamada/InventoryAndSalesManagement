using InventoryAndSalesManagement.Infrastructure.Data;
using InventoryAndSalesManagement.Infrastructure.Repositories;

namespace InventoryAndSalesManagement.Features.Customers
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context) { }
    }
}
