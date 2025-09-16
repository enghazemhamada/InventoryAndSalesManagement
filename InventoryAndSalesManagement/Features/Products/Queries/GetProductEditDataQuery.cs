using MediatR;

namespace InventoryAndSalesManagement.Features.Products.Queries
{
    public record GetProductEditDataQuery(int id) : IRequest<EditProductViewModel>;
}
