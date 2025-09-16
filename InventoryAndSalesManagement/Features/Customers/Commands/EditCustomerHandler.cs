using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;

namespace InventoryAndSalesManagement.Features.Customers.Commands
{
    public class EditCustomerHandler : IRequestHandler<EditCustomerCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public EditCustomerHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(EditCustomerCommand request, CancellationToken cancellationToken)
        {
            Customer customerFromDB = await _context.Customers.FindAsync(request.id);
            if(customerFromDB != null)
            {
                customerFromDB.Name = request.customerVM.Name;
                customerFromDB.Email = request.customerVM.Email;
                customerFromDB.Phone = request.customerVM.Phone;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
