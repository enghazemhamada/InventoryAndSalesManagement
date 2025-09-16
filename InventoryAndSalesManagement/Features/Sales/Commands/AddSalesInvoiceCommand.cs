using MediatR;

namespace InventoryAndSalesManagement.Features.Sales.Commands
{
    public record AddSalesInvoiceCommand(SalesInvoiceWithCustomersWithProductsViewModel salesInvoiceVM) : IRequest<bool>;
}
