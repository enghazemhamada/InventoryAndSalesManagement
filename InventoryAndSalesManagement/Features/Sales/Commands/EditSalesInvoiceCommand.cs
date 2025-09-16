using MediatR;

namespace InventoryAndSalesManagement.Features.Sales.Commands
{
    public record EditSalesInvoiceCommand(int id, EditSalesInvoiceWithCustomersWithProductsViewModel salesInvoiceVM) : IRequest<bool>;
}
