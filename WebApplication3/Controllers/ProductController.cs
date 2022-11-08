using DataAccessLayer.Entity;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Mappers.MapperInterface;
using WebApplication3.RequestsModels.RequestModels;
using WebApplication3.UserViewRequestsModel;

namespace WebApplication3.Controllers
{
    // add functions from repository with handling exceptions
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _prouductRepository;
        private readonly IMapper<Product, ProductViewRequestModel> _productMapper;
        public ProductsController(IProductRepository prouductrepository, ILogger<ProductsController> logger, IMapper<Product, ProductViewRequestModel> productMapper)
        {
            _prouductRepository = prouductrepository;
            _logger = logger;
            _productMapper = productMapper;
        }

        [Route("{Id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProductByIdAsync(int id)
        {
            await _prouductRepository.RemoveProductByIdAsync(id);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddtProductAsync(ProductRequestModel product)
        {
            var newProduct = new Product()
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.ParentCategoryId, //change to category id
            };
            //newProduct.Id = product.Id;
            //newProduct.Name = product.Name;
            //newProduct.Description = product.Description;
            //newProduct.CategoryId = product.
            await _prouductRepository.AddProductAsync(newProduct);
            return Ok(newProduct);
        }

        [Route("ChangeProduct")]
        [HttpPatch]
        public async Task<IActionResult> PatchPartlyOrFullyProductByIdAsync(ProductRequestModel product)
        {
            var expectedProduct = await _prouductRepository.GetProductByIdAsync(product.Id);

            expectedProduct.Description = product.Description;
            expectedProduct.CategoryId = (int)product.ParentCategoryId;
            expectedProduct.Name = product.Name;
            expectedProduct.Price = product.Price;

            await _prouductRepository.UpdateProductAsync(expectedProduct);
            return Ok(expectedProduct);

        }

        [Route("GetProducts")]
        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var expectedProductList = await _prouductRepository.GetAllProductsAsync();
            var productsViewModelList = new List<ProductViewRequestModel>();

            foreach (var expectedProduct in expectedProductList) // LINQ can be used as an option
            {
                var productForUserToView = _productMapper.Map(expectedProduct);
                productsViewModelList.Add(productForUserToView);
            }

            return Ok(productsViewModelList);
        }

        [Route("GetProductById")]
        [HttpGet]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            try
            {
                var expectedProduct = await _prouductRepository.GetProductByIdAsync(id);
                var productsViewModel = _productMapper.Map(expectedProduct);
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
