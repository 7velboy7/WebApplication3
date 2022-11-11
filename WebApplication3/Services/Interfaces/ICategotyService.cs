using WebApplication3.RequestsModels.RequestModels;
using WebApplication3.UserViewRequestsModel;

namespace WebApplication3.Services.Interfaces
{
    public interface ICategotyService
    {
        Task<CategoryRequestModel> CreateProductAsync(CategoryRequestModel productRequest);
        Task<CategoryRequestModel> UpdateProductAsync(CategoryRequestModel productRequest);
        Task DeleteProductByIdAsync(int id);
        Task<CategoryViewRequestModel> GetProductByIdAsync(int id);
        Task<IEnumerable<CategoryViewRequestModel>> GetAllProductAsync();
    }
}
