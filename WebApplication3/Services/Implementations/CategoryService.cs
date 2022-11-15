using DataAccessLayer.Entity;
using DataAccessLayer.Repository.RepositoryInterfaces;
using WebApplication3.DTOs.Responces;
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

        public async Task<CreateCategoryRequestModel> CreateCategoryAsync(CreateCategoryRequestModel categoryRequest)
        {
            var newCategory = new Category()
            {
                Name = categoryRequest.Name,
                ParentCategoryId = categoryRequest.ParentCategoryId
            };
            await _categoryRepository.AddCategoryAsync(newCategory);

            return categoryRequest;
        }

        public async Task DeleteCategoryByIdAsync(int id)
        {
            await _categoryRepository.RemoveCategoryByIdAsync(id);
        }

        public async Task<AllCategoriesWithPaginationResponse> GetAllCategoriesAsync(int page, int itemsPerPage)
        {
            var expectedCategories = await _categoryRepository.GetAllCategoriesAsync();
          
            return new AllCategoriesWithPaginationResponse
            {
                CategoryResponceList = expectedCategories
                    .Skip((page - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .Select(category => _categoryMapper.Map(category))
                    .ToList(),
                CurrentPage = page,
                Pages = (int)Math.Ceiling(expectedCategories.Count() / (double)itemsPerPage)
            };
        }

        public async Task<CategoryViewRequestModel> GetCategoryByIdAsync(int id)
        {
            var expectedCategory = await _categoryRepository.GetCategorytByIdAsync(id);
            if (expectedCategory ==  null)
            {
                return null;
            }
            var categoryViewModel = _categoryMapper.Map(expectedCategory);
            return categoryViewModel;
        }

        public async Task<UpdateCategoryRequestModel> UpdateCategoryAsync(UpdateCategoryRequestModel categoryRequest)
        {
            var expectedCategory = await _categoryRepository.GetCategorytByIdAsync(categoryRequest.Id);
            expectedCategory.Name = categoryRequest.Name;
            expectedCategory.ParentCategoryId = categoryRequest.ParentCategoryId;

            await _categoryRepository.UpdateCategoryAsync(expectedCategory);
            return categoryRequest;
        }
    }
}
