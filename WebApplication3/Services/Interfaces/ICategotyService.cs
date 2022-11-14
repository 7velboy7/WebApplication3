using WebApplication3.RequestsModels.RequestModels;
using WebApplication3.UserViewRequestsModel;

namespace WebApplication3.Services.Interfaces
{
    public interface ICategotyService
    {
        Task<CreateCategoryRequestModel> CreateProductAsync(CreateCategoryRequestModel productRequest);
        Task<UpdateCategoryRequestModel> UpdateProductAsync(UpdateCategoryRequestModel productRequest);
        Task DeleteProductByIdAsync(int id);
        Task<CategoryViewRequestModel> GetProductByIdAsync(int id);
        Task<IEnumerable<CategoryViewRequestModel>> GetAllProductAsync();
    }
}
