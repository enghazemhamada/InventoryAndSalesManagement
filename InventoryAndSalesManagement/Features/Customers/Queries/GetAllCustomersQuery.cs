using MediatR;

namespace InventoryAndSalesManagement.Features.Customers.Queries
{
    public record GetAllCustomersQuery : IRequest<List<Customer>>;
}
