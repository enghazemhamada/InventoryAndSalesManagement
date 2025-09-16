using MediatR;

namespace InventoryAndSalesManagement.Features.Reports.Queries
{
    public record GetReportQuery(DateTime? fromDate, DateTime? toDate) : IRequest<ReportViewModel>;
}
