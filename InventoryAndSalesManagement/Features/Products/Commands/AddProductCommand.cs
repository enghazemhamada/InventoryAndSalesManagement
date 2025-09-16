using MediatR;

namespace InventoryAndSalesManagement.Features.Products.Commands
{
    public record AddProductCommand(AddProductViewModel productVM) : IRequest<Product>;
}
