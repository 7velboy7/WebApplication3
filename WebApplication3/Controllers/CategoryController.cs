using DataAccessLayer.Entity;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTOs.Responces;
using WebApplication3.Mappers.MapperInterface;
using WebApplication3.RequestsModels.RequestModels;
using WebApplication3.Services.Interfaces;

namespace WebApplication3.Controllers
{
    // add functions from repository with handling exceptions
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
           
            var categoryViewModel = await _categoryService.GetProductByIdAsync(id);

            if (categoryViewModel == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            _logger.LogInformation($"Category: {categoryViewModel} was successfully found");
            return Ok(categoryViewModel);

           
              
               
           
        }


        [HttpPost]
        public async Task<IActionResult> AddCategoryAsync(CreateCategoryRequestModel categoryModel)
        {
            var maybeWillModifiedInFutureCategoryRequestModel = await _categoryService.CreateProductAsync(categoryModel);

            _logger.LogInformation($"Product with was deleteded category name: {categoryModel.Name} was added");
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
        public async Task<IActionResult> ChancheCategoryAsync(UpdateCategoryRequestModel categoryModel) //check this
        {
            var maybeWillModifiedInFutureCategoryRequestModel = await _categoryService.UpdateProductAsync(categoryModel);

            _logger.LogInformation($"Category {categoryModel} with id: {categoryModel.Id} was changed");
            return Ok(maybeWillModifiedInFutureCategoryRequestModel);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {

            var maybeModifiedInFutureCategoryList = await _categoryService.GetAllProductAsync(); //add pagination

            _logger.LogInformation($"Gategory list was found. Here all the categories: {maybeModifiedInFutureCategoryList.ToList()}");
            return Ok(new GetAllCategoryResponse
            {
                CategoryResponceList = maybeModifiedInFutureCategoryList.ToList()
            });

        }
    }
}
