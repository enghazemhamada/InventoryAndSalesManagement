using MediatR;

namespace InventoryAndSalesManagement.Features.Products.Commands
{
    public record EditProductCommand(int id, EditProductViewModel productVM) : IRequest<bool>;
}
