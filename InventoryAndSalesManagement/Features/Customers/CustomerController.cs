using Microsoft.AspNetCore.Mvc;

namespace InventoryAndSalesManagement.Features.Customers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Customer> customers = await _customerRepository.GetAllAsync();

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
                Customer customer = new Customer
                {
                    Name = customerVM.Name,
                    Email = customerVM.Email,
                    Phone = customerVM.Phone
                };

                await _customerRepository.AddAsync(customer);
                await _customerRepository.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("/Features/Customers/Views/Add.cshtml", customerVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Customer customerFromDB = await _customerRepository.GetByIdAsync(id);
            if(customerFromDB != null)
            {
                EditCustomerViewModel customerVM = new EditCustomerViewModel
                {
                    Id = id,
                    Name = customerFromDB.Name,
                    Email = customerFromDB.Email,
                    Phone = customerFromDB.Phone
                };

                return View("/Features/Customers/Views/Edit.cshtml", customerVM);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(int id, EditCustomerViewModel customerVM)
        {
            if(ModelState.IsValid)
            {
                Customer customerFromDB = await _customerRepository.GetByIdAsync(id);
                if(customerFromDB != null)
                {
                    customerFromDB.Name = customerVM.Name;
                    customerFromDB.Email = customerVM.Email;
                    customerFromDB.Phone = customerVM.Phone;

                    await _customerRepository.SaveAsync();
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            return View("/Features/Customers/Views/Edit.cshtml", customerVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Customer customerFromDB = await _customerRepository.GetByIdAsync(id);
            if(customerFromDB != null)
            {
                return View("/Features/Customers/Views/Delete.cshtml", customerFromDB);
            }
            return NotFound();
        }

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            Customer customerFromDB = await _customerRepository.GetByIdAsync(id);
            if(customerFromDB != null)
            {
                _customerRepository.Delete(customerFromDB);
                await _customerRepository.SaveAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
