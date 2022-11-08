using DataAccessLayer.Entity;
using WebApplication3.Mappers.MapperInterface;
using WebApplication3.UserViewRequestsModel;

namespace WebApplication3.Mappers.MapperImplementation
{
    public class ProductMapper : IMapper<Product, ProductViewRequestModel>
    {
        public ProductViewRequestModel Map(Product toMap)
        {
            ProductViewRequestModel viewModel = new ProductViewRequestModel();
            viewModel.Name = toMap.Name;
            viewModel.Price = toMap.Price;
            viewModel.Description = toMap.Description;
            viewModel.ParentCategoryId = toMap.CategoryId;
            return viewModel;
        }
    }
}
