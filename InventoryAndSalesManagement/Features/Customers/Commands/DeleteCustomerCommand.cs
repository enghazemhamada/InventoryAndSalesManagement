using MediatR;

namespace InventoryAndSalesManagement.Features.Customers.Commands
{
    public record DeleteCustomerCommand(int id) : IRequest<bool>;
}
