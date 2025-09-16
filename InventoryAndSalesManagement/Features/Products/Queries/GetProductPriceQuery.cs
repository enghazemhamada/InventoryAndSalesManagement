using MediatR;

namespace InventoryAndSalesManagement.Features.Products.Queries
{
    public record GetProductPriceQuery(int id) : IRequest<ProductPriceViewModel>;
}
