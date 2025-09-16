using MediatR;

namespace InventoryAndSalesManagement.Features.Sales.Commands
{
    public record DeleteSalesInvoiceCommand(int id) : IRequest<bool>;
}
