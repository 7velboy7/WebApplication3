using DataAccessLayer.Entity;
using DataAccessLayer.Repository.RepositoryInterfaces;
using WebApplication3.Mappers.MapperInterface;
using WebApplication3.RequestsModels.RequestModels;
using WebApplication3.Services.Interfaces;
using WebApplication3.UserViewRequestsModel;

namespace WebApplication3.Services.Implementations
{
    public class CategoryService : ICategotyService
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper<Category, CategoryViewRequestModel> _categoryMapper; //connects view models with DB models

        public CategoryService(IMapper<Category, CategoryViewRequestModel> categoryMapper, ICategoryRepository categoryRepository)
        {
            _categoryMapper = categoryMapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryRequestModel> CreateProductAsync(CategoryRequestModel categoryRequest)
        {
            var newCategory = new Category()
            {
                Name = categoryRequest.Name,
                ParentCategoryId = categoryRequest.ParentCategoryId
            };
            await _categoryRepository.AddCategoryAsync(newCategory);

            return categoryRequest;
        }

        public async Task DeleteProductByIdAsync(int id)
        {
            await _categoryRepository.RemoveCategoryByIdAsync(id);
        }

        public async Task<IEnumerable<CategoryViewRequestModel>> GetAllProductAsync()
        {
            var expectedCategories = await _categoryRepository.GetAllCategoriesAsync();
            var categoriesViewModelList = new List<CategoryViewRequestModel>();

            foreach (var expectedCategory in expectedCategories)
            {
                var categoryViewModelresult = _categoryMapper.Map(expectedCategory);
                categoriesViewModelList.Add(categoryViewModelresult);
            }

            return categoriesViewModelList;
        }

        public async Task<CategoryViewRequestModel> GetProductByIdAsync(int id)
        {
            var expectedCategory = await _categoryRepository.GetCategorytByIdAsync(id);
            var categoryViewModel = _categoryMapper.Map(expectedCategory);
            return categoryViewModel;
        }

        public async Task<CategoryRequestModel> UpdateProductAsync(CategoryRequestModel categoryRequest)
        {
            var expectedCategory = await _categoryRepository.GetCategorytByIdAsync(categoryRequest.Id);
            expectedCategory.Name = categoryRequest.Name;
            expectedCategory.ParentCategoryId = categoryRequest.ParentCategoryId;

            await _categoryRepository.UpdateCategoryAsync(expectedCategory);
            return categoryRequest;
        }
    }
}
