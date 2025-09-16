using MediatR;

namespace InventoryAndSalesManagement.Features.Products.Commands
{
    public record DeleteProductCommand(int id) : IRequest<bool>;
}
