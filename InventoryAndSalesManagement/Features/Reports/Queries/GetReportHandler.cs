using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAndSalesManagement.Features.Reports.Queries
{
    public class GetReportHandler : IRequestHandler<GetReportQuery, ReportViewModel>
    {
        private readonly ApplicationDbContext _context;

        public GetReportHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReportViewModel> Handle(GetReportQuery request, CancellationToken cancellationToken)
        {
            var query = _context.SalesInvoices.Include(i => i.Customer).Include(i => i.Items).ThenInclude(it => it.Product).AsQueryable();

            if(request.fromDate.HasValue)
            {
                var from = request.fromDate.Value.Date;
                query = query.Where(i => i.Date >= from);
            }

            if(request.toDate.HasValue)
            {
                var to = request.toDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(i => i.Date <= to);
            }

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
            return report;
        }
    }
}
