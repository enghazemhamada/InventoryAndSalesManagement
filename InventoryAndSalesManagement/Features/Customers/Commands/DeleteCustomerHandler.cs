using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;

namespace InventoryAndSalesManagement.Features.Customers.Commands
{
    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeleteCustomerHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            Customer customerFromDB = await _context.Customers.FindAsync(request.id);
            if(customerFromDB != null)
            {
                _context.Customers.Remove(customerFromDB);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
