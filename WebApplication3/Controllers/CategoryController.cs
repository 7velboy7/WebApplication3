using DataAccessLayer.Entity;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Mappers.MapperInterface;
using WebApplication3.RequestsModels.RequestModels;
using WebApplication3.UserViewRequestsModel;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper<Category, CategoryViewRequestModel> _categoryMapper; //connects view models with DB models

        public CategoriesController(ICategoryRepository categoryRepository, ILogger<CategoriesController> logger, IMapper<Category, CategoryViewRequestModel> categoryMapper)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _categoryMapper = categoryMapper;
        }

        [Route("{id}")] // can be not used
        [HttpGet]
        public async Task<IActionResult> GetCategoryByID(int id) // should be from query explicitly
        {
            try
            {
                var expectedCategory = await _categoryRepository.GetCategorytByIdAsync(id);
                var categoryViewModel = _categoryMapper.Map(expectedCategory);
                return Ok(categoryViewModel);
            }
            catch (CategoryNotFoundException ex)
            {
                _logger.LogError(ex, $"My own message");
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
            var newCategory = new Category()
            {
                Name = categoryModel.Name,
                ParentCategoryId = categoryModel.ParentCategoryId
            };
            await _categoryRepository.AddCategoryAsync(newCategory);

            return Ok(newCategory);

            // GET all categories from DB
            // Check that categoryModel.Name does not exist in the list from DB

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            await _categoryRepository.RemoveCategoryByIdAsync(id);
            return NoContent();

        }


        [HttpPatch]
        public async Task<IActionResult> ChanchePartlyOrFullyCategoryAsync(CategoryRequestModel categoryModel) //check this
        {
            var expectedCategory = await _categoryRepository.GetCategorytByIdAsync(categoryModel.Id);
            expectedCategory.Name = categoryModel.Name;
            expectedCategory.ParentCategoryId = categoryModel.ParentCategoryId;

            return Ok(expectedCategory);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var expectedCategories = await _categoryRepository.GetAllCategoriesAsync();
            var categoriesViewModelList = new List<CategoryViewRequestModel>();

            foreach (var expectedCategory in expectedCategories)
            {
                var categoryViewModelresult = _categoryMapper.Map(expectedCategory);
                categoriesViewModelList.Add(categoryViewModelresult);
            }

            return Ok(categoriesViewModelList);

        }
    }
}
