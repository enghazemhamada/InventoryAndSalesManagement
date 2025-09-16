using MediatR;

namespace InventoryAndSalesManagement.Features.Customers.Commands
{
    public record EditCustomerCommand(int id, EditCustomerViewModel customerVM) : IRequest<bool>;
}
