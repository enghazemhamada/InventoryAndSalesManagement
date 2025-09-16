using MediatR;

namespace InventoryAndSalesManagement.Features.Sales.Queries
{
    public record GetAllSalesInvoicesQuery : IRequest<List<SalesInvoice>>;
}
