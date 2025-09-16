using InventoryAndSalesManagement.Features.Products;
using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;

namespace InventoryAndSalesManagement.Features.Sales.Commands
{
    public class AddSalesInvoiceHandler : IRequestHandler<AddSalesInvoiceCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public AddSalesInvoiceHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AddSalesInvoiceCommand request, CancellationToken cancellationToken)
        {
            foreach(var item in request.salesInvoiceVM.Items)
            {
                Product product = await _context.Products.FindAsync(item.ProductId);
                if(product == null)
                    return false;

                if(item.Quantity > product.QuantityInStock)
                    throw new Exception($"Not enough stock for product: {product.Name}");
            }

            DateTime now = DateTime.Now;
            string dateStr = now.ToString("ddHHmmss");
            int invoiceNo = int.Parse(dateStr + request.salesInvoiceVM.CustomerId);

            decimal total = 0;
            List<SalesInvoiceItem> items = new List<SalesInvoiceItem>();
            foreach(var item in request.salesInvoiceVM.Items)
            {
                Product product = await _context.Products.FindAsync(item.ProductId);
                total += item.Quantity * product.Price;
                items.Add(new SalesInvoiceItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                });
            }

            SalesInvoice salesInvoice = new SalesInvoice
            {
                InvoiceNo = invoiceNo,
                CustomerId = request.salesInvoiceVM.CustomerId,
                Total = total,
                Items = items
            };

            await _context.SalesInvoices.AddAsync(salesInvoice);
            await _context.SaveChangesAsync();

            foreach(var item in request.salesInvoiceVM.Items)
            {
                Product product = await _context.Products.FindAsync(item.ProductId);
                product.QuantityInStock -= item.Quantity;
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
