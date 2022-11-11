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
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategotyService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ILogger<CategoriesController> logger, ICategotyService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [Route("{id}")] 
        [HttpGet]
        public async Task<IActionResult> GetCategoryByID(int id) // should be from query explicitly
        {
            try
            {
                var categoryViewModel = await _categoryService.GetProductByIdAsync(id);
                return Ok(categoryViewModel);
            }
            catch (CategoryNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message, $"My own message"); //send message from the method
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request has been failed");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddCategoryAsync(CategoryRequestModel categoryModel)
        {
            var maybeWillModifiedInFutureCategoryRequestModel = await _categoryService.CreateProductAsync(categoryModel);

            _logger.LogInformation($"Product with was deleteded category id: {categoryModel.Id} was added");
            return Ok(maybeWillModifiedInFutureCategoryRequestModel);

            // GET all categories from DB
            // Check that categoryModel.Name does not exist in the list from DB
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
           await _categoryService.DeleteProductByIdAsync(id);

            _logger.LogInformation($"Category with id:{id} was removed");
            return NoContent();

        }


        [HttpPatch]
        public async Task<IActionResult> ChancheCategoryAsync(CategoryRequestModel categoryModel) //check this
        {
            var maybeWillModifiedInFutureCategoryRequestModel = await _categoryService.UpdateProductAsync(categoryModel);

            _logger.LogInformation($"Category {categoryModel} with id: {categoryModel.Id} was changed");
            return Ok(maybeWillModifiedInFutureCategoryRequestModel);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {

            var maybeModifiedInFutureategoryList = await _categoryService.GetAllProductAsync();

            _logger.LogInformation($"Gategory list was found. Here all the categories: {maybeModifiedInFutureategoryList.ToList()}");
            return Ok(maybeModifiedInFutureategoryList);

        }
    }
}
