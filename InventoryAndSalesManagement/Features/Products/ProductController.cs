using Microsoft.AspNetCore.Mvc;

namespace InventoryAndSalesManagement.Features.Products
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _productRepository.GetAllAsync();

            return View("/Features/Products/Views/Index.cshtml", products);
        }

        public async Task<IActionResult> Search(string search)
        {
            IEnumerable<Product> products = await _productRepository.GetAllAsync();
            if(!string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(p => p.Name.Contains(search));
            }

            return PartialView("/Features/Products/Views/_ProductsTable.cshtml", products);
        }

        public IActionResult Add()
        {
            return View("/Features/Products/Views/Add.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> SaveAdd(AddProductViewModel productVM)
        {
            if(ModelState.IsValid)
            {
                Product product = new Product
                {
                    Name = productVM.Name,
                    Price = productVM.Price,
                    QuantityInStock = productVM.QuantityInStock
                };

                await _productRepository.AddAsync(product);
                await _productRepository.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("/Features/Products/Views/Add.cshtml", productVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Product productFromDB = await _productRepository.GetByIdAsync(id);
            if(productFromDB != null)
            {
                EditProductViewModel productVM = new EditProductViewModel
                {
                    Id = id,
                    Name = productFromDB.Name,
                    Price = productFromDB.Price,
                    QuantityInStock = productFromDB.QuantityInStock
                };

                return View("/Features/Products/Views/Edit.cshtml", productVM);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(int id, EditProductViewModel productVM)
        {
            if(ModelState.IsValid)
            {
                Product productFromDB = await _productRepository.GetByIdAsync(id);
                if(productFromDB != null)
                {
                    productFromDB.Name = productVM.Name;
                    productFromDB.Price = productVM.Price;
                    productFromDB.QuantityInStock = productVM.QuantityInStock;

                    await _productRepository.SaveAsync();
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            return View("/Features/Products/Views/Edit.cshtml", productVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Product productFromDB = await _productRepository.GetByIdAsync(id);
            if(productFromDB != null)
            {
                return View("/Features/Products/Views/Delete.cshtml", productFromDB);
            }
            return NotFound();
        }

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            Product productFromDB = await _productRepository.GetByIdAsync(id);
            if(productFromDB != null)
            {
                _productRepository.Delete(productFromDB);
                await _productRepository.SaveAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
