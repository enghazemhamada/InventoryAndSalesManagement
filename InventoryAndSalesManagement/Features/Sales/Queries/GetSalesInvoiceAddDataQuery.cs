using MediatR;

namespace InventoryAndSalesManagement.Features.Sales.Queries
{
    public record GetSalesInvoiceAddDataQuery : IRequest<SalesInvoiceWithCustomersWithProductsViewModel>;
}
