using DataAccessLayer.Entity;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Mappers.MapperInterface;
using WebApplication3.RequestsModels.RequestModels;
using WebApplication3.Services.Interfaces;
using WebApplication3.UserViewRequestsModel;

namespace WebApplication3.Controllers
{
    // add functions from repository with handling exceptions
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IProductService _productService;
        public ProductsController(IProductRepository prouductrepository, ILogger<ProductsController> logger, IMapper<Product, ProductViewRequestModel> productMapper, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [Route("{Id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProductByIdAsync(int id)
        {
            await _productService.DeleteProductByIdAsync(id);  

            _logger.LogInformation($"Product with productId: {id} was deleted");
            return NoContent();
        }

        [HttpPost] 
        public async Task<IActionResult> AddProductAsync(ProductRequestModel product)
        {
            var maybeModifiedInFutureProduct = await _productService.CreateProductAsync(product);
            //oblochit obolochkoi try catch
            _logger.LogInformation($"Product: {product} with productId: {product.Id} was added");
            return Ok(maybeModifiedInFutureProduct);
        }

        
        [HttpPatch] 
        public async Task<IActionResult> PatchProductAsync(ProductRequestModel product)
        {
             var maybeModifiedInFutureProduct = await _productService.UpdateProductAsync(product);

            _logger.LogInformation($"Product: {product} with productId{product.Id} was changed");
            return Ok(maybeModifiedInFutureProduct); //here we return the same sheet that we had at the beginning
        }

        
        [HttpGet] 
        public async Task<IActionResult> GetAllProductsAsync([FromQuery] int page)
        {
            const int itemsPerPage = 20;

            var productsViewModelList = await _productService.GetAllProductAsync(page, itemsPerPage);

            _logger.LogInformation($"Products list was found. Here all the products: {productsViewModelList}");
            return Ok(productsViewModelList);
        }

        [Route("{Id}")] 
        [HttpGet]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var productsViewModel = await _productService.GetProductByIdAsync(id); // return null and handl null TODO

            if (productsViewModel == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            _logger.LogInformation($"Product: {productsViewModel} was found");
            return Ok(productsViewModel);

        }
    }
}
