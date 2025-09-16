using InventoryAndSalesManagement.Features.Customers.Commands;
using InventoryAndSalesManagement.Features.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAndSalesManagement.Features.Customers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            List<Customer> customers = await _mediator.Send(new GetAllCustomersQuery());

            return View("/Features/Customers/Views/Index.cshtml", customers);
        }

        public IActionResult Add()
        {
            return View("/Features/Customers/Views/Add.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> SaveAdd(AddCustomerViewModel customerVM)
        {
            if(ModelState.IsValid)
            {
                Customer customer = await _mediator.Send(new AddCustomerCommand(customerVM));
                return RedirectToAction("Index");
            }
            return View("/Features/Customers/Views/Add.cshtml", customerVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            EditCustomerViewModel customerVM = await _mediator.Send(new GetCustomerEditDataQuery(id));
            if(customerVM != null)
            {
                return View("/Features/Customers/Views/Edit.cshtml", customerVM);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(int id, EditCustomerViewModel customerVM)
        {
            if(ModelState.IsValid)
            {
                bool result = await _mediator.Send(new EditCustomerCommand(id, customerVM));
                if(result)
                    return RedirectToAction("Index");

                return NotFound();
            }
            return View("/Features/Customers/Views/Edit.cshtml", customerVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Customer customerFromDB = await _mediator.Send(new GetCustomerByIdQuery(id));
            if(customerFromDB != null)
            {
                return View("/Features/Customers/Views/Delete.cshtml", customerFromDB);
            }
            return NotFound();
        }

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            bool result = await _mediator.Send(new DeleteCustomerCommand(id));
            if(result)
                return RedirectToAction("Index");

            return NotFound();
        }
    }
}
