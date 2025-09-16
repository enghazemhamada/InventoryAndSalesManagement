using MediatR;

namespace InventoryAndSalesManagement.Features.Customers.Queries
{
    public record GetCustomerEditDataQuery(int id) : IRequest<EditCustomerViewModel>;
}
