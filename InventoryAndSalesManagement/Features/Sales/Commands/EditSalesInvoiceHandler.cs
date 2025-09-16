using InventoryAndSalesManagement.Features.Products;
using InventoryAndSalesManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAndSalesManagement.Features.Sales.Commands
{
    public class EditSalesInvoiceHandler : IRequestHandler<EditSalesInvoiceCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public EditSalesInvoiceHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(EditSalesInvoiceCommand request, CancellationToken cancellationToken)
        {
            SalesInvoice salesInvoice = await _context.SalesInvoices.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == request.id);
            if(salesInvoice != null)
            {
                foreach(var item in request.salesInvoiceVM.Items)
                {
                    Product product = await _context.Products.FindAsync(item.ProductId);
                    if(product == null)
                        return false;

                    if(item.Quantity > product.QuantityInStock)
                        throw new Exception($"Not enough stock for product: {product.Name}");
                }

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

                for(int i = 0; i < request.salesInvoiceVM.Items.Count; i++)
                {
                    Product product = await _context.Products.FindAsync(request.salesInvoiceVM.Items[i].ProductId);
                    if(salesInvoice.Items.Count <= i)
                    {
                        product.QuantityInStock -= request.salesInvoiceVM.Items[i].Quantity;
                    }
                    else if(salesInvoice.Items[i].Quantity > request.salesInvoiceVM.Items[i].Quantity)
                    {
                        int count = salesInvoice.Items[i].Quantity - request.salesInvoiceVM.Items[i].Quantity;
                        product.QuantityInStock += count;
                    }
                    else if(salesInvoice.Items[i].Quantity < request.salesInvoiceVM.Items[i].Quantity)
                    {
                        int count = request.salesInvoiceVM.Items[i].Quantity - salesInvoice.Items[i].Quantity;
                        product.QuantityInStock -= count;
                    }
                }

                salesInvoice.CustomerId = request.salesInvoiceVM.CustomerId;
                salesInvoice.Total = total;
                salesInvoice.Items = items;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
