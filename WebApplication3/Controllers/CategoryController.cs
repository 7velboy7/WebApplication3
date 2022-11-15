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
           
            var categoryViewModel = await _categoryService.GetCategoryByIdAsync(id);

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
            var maybeWillModifiedInFutureCategoryRequestModel = await _categoryService.CreateCategoryAsync(categoryModel);

            _logger.LogInformation($"Product with was deleteded category name: {categoryModel.Name} was added");
            return Ok(maybeWillModifiedInFutureCategoryRequestModel);

            // GET all categories from DB
            // Check that categoryModel.Name does not exist in the list from DB
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
           await _categoryService.DeleteCategoryByIdAsync(id);

            _logger.LogInformation($"Category with id:{id} was removed");
            return NoContent();

        }


        [HttpPatch]
        public async Task<IActionResult> ChangeCategoryAsync(UpdateCategoryRequestModel categoryModel) //check this
        {
            var maybeWillModifiedInFutureCategoryRequestModel = await _categoryService.UpdateCategoryAsync(categoryModel);

            _logger.LogInformation($"Category {categoryModel} with id: {categoryModel.Id} was changed");
            return Ok(maybeWillModifiedInFutureCategoryRequestModel);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync([FromQuery]int page)
        {
            const int itemsPerPage = 5;
            var maybeModifiedInFutureCategoryList = await _categoryService.GetAllCategoriesAsync(page, itemsPerPage); //add pagination


            _logger.LogInformation($"Gategory list was found. Here all the categories via pagination: {maybeModifiedInFutureCategoryList}");

            
            return Ok(maybeModifiedInFutureCategoryList);

        }
    }
}
