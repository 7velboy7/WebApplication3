
using WebApplication3.DTOs.Responces;
using WebApplication3.RequestsModels.RequestModels;
using WebApplication3.UserViewRequestsModel;

namespace WebApplication3.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductViewRequestModel> CreateProductAsync(ProductRequestModel productRequest);
        Task<ProductRequestModel> UpdateProductAsync(ProductRequestModel productRequest);
        Task DeleteProductByIdAsync(int id);
        Task<ProductViewRequestModel> GetProductByIdAsync(int id);
        Task<AllProductsWithPaginationResponse> GetAllProductAsync(int page, int itemPerPage);
    }
}
