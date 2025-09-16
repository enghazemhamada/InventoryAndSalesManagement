using MediatR;

namespace InventoryAndSalesManagement.Features.Customers.Commands
{
    public record AddCustomerCommand(AddCustomerViewModel customerVM) : IRequest<Customer>;
}
