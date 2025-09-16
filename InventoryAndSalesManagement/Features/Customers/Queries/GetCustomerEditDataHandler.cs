using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;

namespace InventoryAndSalesManagement.Features.Customers.Queries
{
    public class GetCustomerEditDataHandler : IRequestHandler<GetCustomerEditDataQuery, EditCustomerViewModel>
    {
        private readonly ApplicationDbContext _context;

        public GetCustomerEditDataHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EditCustomerViewModel> Handle(GetCustomerEditDataQuery request, CancellationToken cancellationToken)
        {
            Customer customerFromDB = await _context.Customers.FindAsync(request.id);
            if(customerFromDB != null)
            {
                EditCustomerViewModel customerVM = new EditCustomerViewModel
                {
                    Id = customerFromDB.Id,
                    Name = customerFromDB.Name,
                    Email = customerFromDB.Email,
                    Phone = customerFromDB.Phone
                };
                return customerVM;
            }
            return null;
        }
    }
}
