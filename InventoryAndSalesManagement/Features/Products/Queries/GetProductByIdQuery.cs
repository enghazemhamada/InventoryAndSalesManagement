using MediatR;

namespace InventoryAndSalesManagement.Features.Products.Queries
{
    public record GetProductByIdQuery(int id) : IRequest<Product>;
}
