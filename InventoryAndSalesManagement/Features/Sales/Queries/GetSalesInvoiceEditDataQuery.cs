using MediatR;

namespace InventoryAndSalesManagement.Features.Sales.Queries
{
    public record GetSalesInvoiceEditDataQuery(int id) : IRequest<EditSalesInvoiceWithCustomersWithProductsViewModel>;
}
