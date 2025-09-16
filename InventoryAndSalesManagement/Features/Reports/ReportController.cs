using InventoryAndSalesManagement.Features.Reports.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAndSalesManagement.Features.Reports
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View("/Features/Reports/Views/index.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> GetReport(DateTime? fromDate, DateTime? toDate)
        {
            ReportViewModel report = await _mediator.Send(new GetReportQuery(fromDate, toDate));

            return PartialView("/Features/Reports/Views/_ReportResult.cshtml", report);
        }
    }
}
