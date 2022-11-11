using DataAccessLayer.Entity;
using DataAccessLayer.Repository.RepositoryInterfaces;
using WebApplication3.Mappers.MapperInterface;
using WebApplication3.RequestsModels.RequestModels;
using WebApplication3.Services.Interfaces;
using WebApplication3.UserViewRequestsModel;

namespace WebApplication3.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _prouductRepository;
        private readonly IMapper<Product, ProductViewRequestModel> _productMapper;

        public ProductService(IProductRepository prouductRepository, IMapper<Product, ProductViewRequestModel> productMapper)
        {
            _prouductRepository = prouductRepository;
            _productMapper = productMapper;
        }


        public async Task DeleteProductByIdAsync(int id)
        {
            await _prouductRepository.RemoveProductByIdAsync(id);
        }

        public async Task<IEnumerable<ProductViewRequestModel>> GetAllProductAsync()
        {
            var expectedProductList = await _prouductRepository.GetAllProductsAsync();
            var productsViewModelList = new List<ProductViewRequestModel>();

            foreach (var expectedProduct in expectedProductList) // LINQ can be used as an option
            {
                var productForUserToView = _productMapper.Map(expectedProduct);
                productsViewModelList.Add(productForUserToView);
            }

            return productsViewModelList;
        }

        public async Task<ProductViewRequestModel> GetProductByIdAsync(int id)
        {
            var expectedProduct = await _prouductRepository.GetProductByIdAsync(id);
            var productsViewModel = _productMapper.Map(expectedProduct);
            return productsViewModel;
        }

        public async Task<ProductRequestModel> UpdateProductAsync(ProductRequestModel product)
        {
            var expectedProduct = await _prouductRepository.GetProductByIdAsync(product.Id);

            expectedProduct.Description = product.Description;
            expectedProduct.CategoryId = (int)product.ParentCategoryId;
            expectedProduct.Name = product.Name;
            expectedProduct.Price = product.Price;

            await _prouductRepository.UpdateProductAsync(expectedProduct);

            return product;
        }

        public async Task<ProductViewRequestModel> CreateProductAsync(ProductRequestModel productRequest)
        {
            var newProduct = new Product()
            {
                Name = productRequest.Name,
                CategoryId = productRequest.ParentCategoryId
            };
            await _prouductRepository.AddProductAsync(newProduct);
            var productViewModelRequest = _productMapper.Map(newProduct);

            return productViewModelRequest;

            // send email
        }
    }
}
