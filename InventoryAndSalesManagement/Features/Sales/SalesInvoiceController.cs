using InventoryAndSalesManagement.Features.Customers;
using InventoryAndSalesManagement.Features.Products;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAndSalesManagement.Features.Sales
{
    public class SalesInvoiceController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISalesInvoiceRepository _salesInvoiceRepository;

        public SalesInvoiceController(ICustomerRepository customerRepository, IProductRepository productRepository, ISalesInvoiceRepository salesInvoiceRepository)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _salesInvoiceRepository = salesInvoiceRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<SalesInvoice> salesInvoices = await _salesInvoiceRepository.GetAllSalesInvoicesWithCustomerAsync();
            return View("/Features/Sales/Views/Index.cshtml", salesInvoices);
        }

        public async Task<IActionResult> Add()
        {
            SalesInvoiceWithCustomersWithProductsViewModel salesInvoiceWithCustomersWithProductsVM
                = new SalesInvoiceWithCustomersWithProductsViewModel
                {
                    Customers = await _customerRepository.GetAllAsync(),
                    Products = await _productRepository.GetAllAsync()
                };

            return View("/Features/Sales/Views/Add.cshtml", salesInvoiceWithCustomersWithProductsVM);
        }

        public async Task<IActionResult> GetProductPrice(int productId)
        {
            Product product = await _productRepository.GetByIdAsync(productId);
            if(product != null)
            {
                return Json(new { price = product.Price, stock = product.QuantityInStock });
            }
            return NotFound();
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveAdd(SalesInvoiceWithCustomersWithProductsViewModel salesInvoiceVM)
        {
            if(ModelState.IsValid)
            {
                foreach(var item in salesInvoiceVM.Items)
                {
                    Product product = await _productRepository.GetByIdAsync(item.ProductId);
                    if(item.Quantity > product.QuantityInStock)
                    {
                        ModelState.AddModelError("", $"Not enough stock for product: {product.Name}");

                        salesInvoiceVM.Customers = await _customerRepository.GetAllAsync();
                        salesInvoiceVM.Products = await _productRepository.GetAllAsync();
                        return View("/Features/Sales/Views/Add.cshtml", salesInvoiceVM);
                    }
                }

                DateTime now = DateTime.Now;
                string dateStr = now.ToString("ddHHmmss");
                int invoiceNo = int.Parse(dateStr + salesInvoiceVM.CustomerId);

                SalesInvoice salesInvoice = new SalesInvoice
                {
                    InvoiceNo = invoiceNo,
                    CustomerId = salesInvoiceVM.CustomerId,
                    Total = salesInvoiceVM.Items.Sum(x => x.Quantity * x.Price),
                    Items = salesInvoiceVM.Items.Select(x => new SalesInvoiceItem
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Price
                    }).ToList()
                };

                try
                {
                    await _salesInvoiceRepository.AddAsync(salesInvoice);
                    await _salesInvoiceRepository.SaveAsync();

                    foreach(var item in salesInvoiceVM.Items)
                    {
                        Product product = await _productRepository.GetByIdAsync(item.ProductId);
                        product.QuantityInStock -= item.Quantity;
                    }
                    await _salesInvoiceRepository.SaveAsync();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);

                    salesInvoiceVM.Customers = await _customerRepository.GetAllAsync();
                    salesInvoiceVM.Products = await _productRepository.GetAllAsync();
                    return View("/Features/Sales/Views/Add.cshtml", salesInvoiceVM);
                }
            }
            salesInvoiceVM.Customers = await _customerRepository.GetAllAsync();
            salesInvoiceVM.Products = await _productRepository.GetAllAsync();
            return View("/Features/Sales/Views/Add.cshtml", salesInvoiceVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            SalesInvoice salesInvoice = await _salesInvoiceRepository.GetSalesInvoiceWithCustomerAsync(id);
            if(salesInvoice != null)
            {
                return View("/Features/Sales/Views/Delete.cshtml", salesInvoice);
            }
            return NotFound();
        }

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            SalesInvoice salesInvoice = await _salesInvoiceRepository.GetSalesInvoiceWithItemsAsync(id);
            if(salesInvoice != null)
            {
                foreach(var item in salesInvoice.Items)
                {
                    Product product = await _productRepository.GetByIdAsync(item.ProductId);
                    if(product != null)
                        product.QuantityInStock += item.Quantity;
                }

                _salesInvoiceRepository.Delete(salesInvoice);
                await _salesInvoiceRepository.SaveAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
