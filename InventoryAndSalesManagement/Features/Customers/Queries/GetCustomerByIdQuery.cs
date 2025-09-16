using MediatR;

namespace InventoryAndSalesManagement.Features.Customers.Queries
{
    public record GetCustomerByIdQuery(int id) : IRequest<Customer>;
}
