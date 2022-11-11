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

        [Route("{Id}")] //done
        [HttpDelete]
        public async Task<IActionResult> DeleteProductByIdAsync(int id)
        {
            await _productService.DeleteProductByIdAsync(id);  

            _logger.LogInformation("Product was deleted");
            return NoContent();
        }

        [HttpPost] //done
        public async Task<IActionResult> AddProductAsync(ProductRequestModel product)
        {
            var maybeModifiedInFutureProduct = await _productService.CreateProductAsync(product);
            //oblochit obolochkoi try catch
            _logger.LogInformation("Product was added");
            return Ok(maybeModifiedInFutureProduct);
        }

        
        [HttpPatch] //done
        public async Task<IActionResult> PatchProductAsync(ProductRequestModel product)
        {
             var maybeModifiedInFutureProduct = await _productService.UpdateProductAsync(product);

            _logger.LogInformation("Product was changed");
            return Ok(maybeModifiedInFutureProduct); //here we return the same sheet that we had at the beginning
        }

        
        [HttpGet] //done
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var productsViewModelList = await _productService.GetAllProductAsync();

            _logger.LogInformation("Products were shown");
            return Ok(productsViewModelList);
        }

        [Route("{Id}")] // done
        [HttpGet]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            try
            {
               var productsViewModel = await _productService.GetProductByIdAsync(id);

                _logger.LogInformation("Product was shown");
                return Ok(productsViewModel);
            }
            catch (ProductNotFoundException ex)
            {
                _logger.LogInformation(ex, "request has been failed");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "request has been failed");
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }

    }
}
