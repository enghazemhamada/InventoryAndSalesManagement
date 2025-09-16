using MediatR;

namespace InventoryAndSalesManagement.Features.Products.Queries
{
    public record SearchProductsQuery(string search) : IRequest<List<Product>>;
}
