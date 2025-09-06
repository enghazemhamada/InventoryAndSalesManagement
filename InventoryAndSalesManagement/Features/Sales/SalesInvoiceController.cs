using InventoryAndSalesManagement.Features.Customers;
using InventoryAndSalesManagement.Features.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace InventoryAndSalesManagement.Features.Sales
{
    [Authorize]
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
                    if(product != null)
                    {
                        if(item.Quantity > product.QuantityInStock)
                        {
                            ModelState.AddModelError("", $"Not enough stock for product: {product.Name}");

                            salesInvoiceVM.Customers = await _customerRepository.GetAllAsync();
                            salesInvoiceVM.Products = await _productRepository.GetAllAsync();
                            return View("/Features/Sales/Views/Add.cshtml", salesInvoiceVM);
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }

                DateTime now = DateTime.Now;
                string dateStr = now.ToString("ddHHmmss");
                int invoiceNo = int.Parse(dateStr + salesInvoiceVM.CustomerId);

                decimal total = 0;
                List<SalesInvoiceItem> items = new List<SalesInvoiceItem>();
                foreach(var item in salesInvoiceVM.Items)
                {
                    Product product = await _productRepository.GetByIdAsync(item.ProductId);
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
                    CustomerId = salesInvoiceVM.CustomerId,
                    Total = total,
                    Items = items
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

        public async Task<IActionResult> Edit(int id)
        {
            SalesInvoice salesInvoice = await _salesInvoiceRepository.GetSalesInvoiceWithItemsAsync(id);
            if(salesInvoice != null)
            {
                EditSalesInvoiceWithCustomersWithProductsViewModel salesInvoiceWithCustomersWithProductsVM =
                    new EditSalesInvoiceWithCustomersWithProductsViewModel
                    {
                        Id = id,
                        CustomerId = salesInvoice.CustomerId,
                        Items = salesInvoice.Items.Select(i => new SalesInvoiceItemViewModel
                        {
                            ProductId = i.ProductId,
                            Quantity = i.Quantity,
                            Price = i.Price
                        }).ToList(),
                        Customers = await _customerRepository.GetAllAsync(),
                        Products = await _productRepository.GetAllAsync()
                    };

                return View("/Features/Sales/Views/Edit.cshtml", salesInvoiceWithCustomersWithProductsVM);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(int id, EditSalesInvoiceWithCustomersWithProductsViewModel salesInvoiceVM)
        {
            if(ModelState.IsValid)
            {
                SalesInvoice salesInvoice = await _salesInvoiceRepository.GetSalesInvoiceWithItemsAsync(id);
                if(salesInvoice != null)
                {
                    foreach(var item in salesInvoiceVM.Items)
                    {
                        Product product = await _productRepository.GetByIdAsync(item.ProductId);
                        if(product != null)
                        {
                            if(item.Quantity > product.QuantityInStock)
                            {
                                ModelState.AddModelError("", $"Not enough stock for product: {product.Name}");

                                salesInvoiceVM.Customers = await _customerRepository.GetAllAsync();
                                salesInvoiceVM.Products = await _productRepository.GetAllAsync();
                                return View("/Features/Sales/Views/Edit.cshtml", salesInvoiceVM);
                            }
                        }
                        else
                        {
                            return NotFound();
                        }
                    }

                    decimal total = 0;
                    List<SalesInvoiceItem> items = new List<SalesInvoiceItem>();
                    foreach(var item in salesInvoiceVM.Items)
                    {
                        Product product = await _productRepository.GetByIdAsync(item.ProductId);
                        total += item.Quantity * product.Price;
                        items.Add(new SalesInvoiceItem
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = product.Price
                        });
                    }

                    for(int i = 0; i < salesInvoiceVM.Items.Count; i++)
                    {
                        Product product = await _productRepository.GetByIdAsync(salesInvoiceVM.Items[i].ProductId);
                        if(salesInvoice.Items.Count <= i)
                        {
                            product.QuantityInStock -= salesInvoiceVM.Items[i].Quantity;
                        }
                        else if(salesInvoice.Items[i].Quantity > salesInvoiceVM.Items[i].Quantity)
                        {
                            int count = salesInvoice.Items[i].Quantity - salesInvoiceVM.Items[i].Quantity;
                            product.QuantityInStock += count;
                        }
                        else if(salesInvoice.Items[i].Quantity < salesInvoiceVM.Items[i].Quantity)
                        {
                            int count = salesInvoiceVM.Items[i].Quantity - salesInvoice.Items[i].Quantity;
                            product.QuantityInStock -= count;
                        }
                    }

                    salesInvoice.CustomerId = salesInvoiceVM.CustomerId;
                    salesInvoice.Total = total;
                    salesInvoice.Items = items;

                    try
                    {
                        await _salesInvoiceRepository.SaveAsync();
                        return RedirectToAction("Index");
                    }
                    catch(Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);

                        salesInvoiceVM.Customers = await _customerRepository.GetAllAsync();
                        salesInvoiceVM.Products = await _productRepository.GetAllAsync();
                        return View("/Features/Sales/Views/Edit.cshtml", salesInvoiceVM);
                    }
                }
                return NotFound();
            }
            salesInvoiceVM.Customers = await _customerRepository.GetAllAsync();
            salesInvoiceVM.Products = await _productRepository.GetAllAsync();
            return View("/Features/Sales/Views/Edit.cshtml", salesInvoiceVM);
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
