using InventoryAndSalesManagement.Features.Customers.Queries;
using InventoryAndSalesManagement.Features.Products.Queries;
using InventoryAndSalesManagement.Features.Sales.Commands;
using InventoryAndSalesManagement.Features.Sales.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAndSalesManagement.Features.Sales
{
    [Authorize]
    public class SalesInvoiceController : Controller
    {
        private readonly IMediator _mediator;

        public SalesInvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            List<SalesInvoice> salesInvoices = await _mediator.Send(new GetAllSalesInvoicesQuery());

            return View("/Features/Sales/Views/Index.cshtml", salesInvoices);
        }

        public async Task<IActionResult> Add()
        {
            var salesInvoiceVM = await _mediator.Send(new GetSalesInvoiceAddDataQuery());

            return View("/Features/Sales/Views/Add.cshtml", salesInvoiceVM);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveAdd(SalesInvoiceWithCustomersWithProductsViewModel salesInvoiceVM)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    bool result = await _mediator.Send(new AddSalesInvoiceCommand(salesInvoiceVM));
                    if(result)
                        return RedirectToAction("Index");

                    return NotFound();
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            salesInvoiceVM.Customers = await _mediator.Send(new GetAllCustomersQuery());
            salesInvoiceVM.Products = await _mediator.Send(new GetAllProductsQuery());
            return View("/Features/Sales/Views/Add.cshtml", salesInvoiceVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var salesInvoiceVM = await _mediator.Send(new GetSalesInvoiceEditDataQuery(id));
            if(salesInvoiceVM != null)
            {
                return View("/Features/Sales/Views/Edit.cshtml", salesInvoiceVM);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(int id, EditSalesInvoiceWithCustomersWithProductsViewModel salesInvoiceVM)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    bool result = await _mediator.Send(new EditSalesInvoiceCommand(id, salesInvoiceVM));
                    if(result)
                        return RedirectToAction("Index");

                    return NotFound();
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            salesInvoiceVM.Customers = await _mediator.Send(new GetAllCustomersQuery());
            salesInvoiceVM.Products = await _mediator.Send(new GetAllProductsQuery());
            return View("/Features/Sales/Views/Edit.cshtml", salesInvoiceVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            SalesInvoice salesInvoice = await _mediator.Send(new GetSalesInvoiceDeleteDataQuery(id));
            if(salesInvoice != null)
            {
                return View("/Features/Sales/Views/Delete.cshtml", salesInvoice);
            }
            return NotFound();
        }

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            bool result = await _mediator.Send(new DeleteSalesInvoiceCommand(id));
            if(result)
                return RedirectToAction("Index");

            return NotFound();
        }
    }
}
