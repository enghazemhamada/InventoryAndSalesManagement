using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;

namespace InventoryAndSalesManagement.Features.Customers.Commands
{
    public class AddCustomerHandler : IRequestHandler<AddCustomerCommand, Customer>
    {
        private readonly ApplicationDbContext _context;

        public AddCustomerHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            Customer customer = new Customer
            {
                Name = request.customerVM.Name,
                Email = request.customerVM.Email,
                Phone = request.customerVM.Phone
            };

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}
