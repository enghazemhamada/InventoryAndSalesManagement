using InventoryAndSalesManagement.Features.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryAndSalesManagement.Features.Reports
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly ISalesInvoiceRepository _salesInvoiceRepository;

        public ReportController(ISalesInvoiceRepository salesInvoiceRepository)
        {
            _salesInvoiceRepository = salesInvoiceRepository;
        }

        public IActionResult Index()
        {
            return View("/Features/Reports/Views/index.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> GetReport(DateTime? fromDate, DateTime? toDate)
        {
            var query = _salesInvoiceRepository.GetSalesInvoicesWithItemsWithCustomers();

            if(fromDate.HasValue)
                query = query.Where(i => i.Date >= fromDate.Value);

            if(toDate.HasValue)
                query = query.Where(i => i.Date <= toDate.Value);

            var salesInvoices = await query.ToListAsync();

            ReportViewModel report = new ReportViewModel
            {
                TotalSales = salesInvoices.Sum(i => i.Items.Sum(it => it.Quantity * it.Price)),
                NumberOfSales = salesInvoices.Count,

                Products = salesInvoices
                    .SelectMany(i => i.Items)
                    .GroupBy(it => it.Product.Name)
                    .Select(g => new ProductViewModel
                    {
                        ProductName = g.Key,
                        QuantitySold = g.Sum(x => x.Quantity)
                    })
                    .OrderByDescending(x => x.QuantitySold).Take(5).ToList(),

                Customer = salesInvoices
                    .GroupBy(i => i.Customer.Name)
                    .Select(g => new CustomerViewModel
                    {
                        CustomerName = g.Key,
                        TotalPurchases = g.Sum(i => i.Items.Sum(it => it.Quantity * it.Price))
                    })
                    .OrderByDescending(x => x.TotalPurchases).FirstOrDefault()
            };

            return PartialView("/Features/Reports/Views/_ReportResult.cshtml", report);
        }
    }
}
