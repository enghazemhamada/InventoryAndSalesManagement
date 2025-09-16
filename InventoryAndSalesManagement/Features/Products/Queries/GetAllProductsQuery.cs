using MediatR;

namespace InventoryAndSalesManagement.Features.Products.Queries
{
    public record GetAllProductsQuery : IRequest<List<Product>>;
}
