using MediatR;

namespace InventoryAndSalesManagement.Features.Sales.Queries
{
    public record GetSalesInvoiceDeleteDataQuery(int id) : IRequest<SalesInvoice>;
}
