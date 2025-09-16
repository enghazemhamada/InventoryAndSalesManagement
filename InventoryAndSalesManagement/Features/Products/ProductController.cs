using InventoryAndSalesManagement.Features.Products.Commands;
using InventoryAndSalesManagement.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAndSalesManagement.Features.Products
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _mediator.Send(new GetAllProductsQuery());

            return View("/Features/Products/Views/Index.cshtml", products);
        }

        public async Task<IActionResult> Search(string search)
        {
            List<Product> products = await _mediator.Send(new SearchProductsQuery(search));

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
                Product product = await _mediator.Send(new AddProductCommand(productVM));
                return RedirectToAction("Index");
            }
            return View("/Features/Products/Views/Add.cshtml", productVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            EditProductViewModel productVM = await _mediator.Send(new GetProductEditDataQuery(id));
            if(productVM != null)
            {
                return View("/Features/Products/Views/Edit.cshtml", productVM);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(int id, EditProductViewModel productVM)
        {
            if(ModelState.IsValid)
            {
                bool result = await _mediator.Send(new EditProductCommand(id, productVM));
                if(result)
                    return RedirectToAction("Index");

                return NotFound();
            }
            return View("/Features/Products/Views/Edit.cshtml", productVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Product productFromDB = await _mediator.Send(new GetProductByIdQuery(id));
            if(productFromDB != null)
            {
                return View("/Features/Products/Views/Delete.cshtml", productFromDB);
            }
            return NotFound();
        }

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            bool result = await _mediator.Send(new DeleteProductCommand(id));
            if(result)
                return RedirectToAction("Index");

            return NotFound();
        }

        public async Task<IActionResult> GetProductPrice(int productId)
        {
            ProductPriceViewModel productPriceVM = await _mediator.Send(new GetProductPriceQuery(productId));
            if(productPriceVM != null)
            {
                return Json(productPriceVM);
            }
            return NotFound();
        }
    }
}
