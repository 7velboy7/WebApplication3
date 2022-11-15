using WebApplication3.DTOs.Responces;
using WebApplication3.RequestsModels.RequestModels;
using WebApplication3.UserViewRequestsModel;

namespace WebApplication3.Services.Interfaces
{
    public interface ICategotyService
    {
        Task<CreateCategoryRequestModel> CreateCategoryAsync(CreateCategoryRequestModel productRequest);
        Task<UpdateCategoryRequestModel> UpdateCategoryAsync(UpdateCategoryRequestModel productRequest);
        Task DeleteCategoryByIdAsync(int id);
        Task<CategoryViewRequestModel> GetCategoryByIdAsync(int id);
        Task<AllCategoriesWithPaginationResponse> GetAllCategoriesAsync(int page, int itemsPerPage);
    }
}
