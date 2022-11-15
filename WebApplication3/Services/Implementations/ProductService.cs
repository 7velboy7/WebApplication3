using DataAccessLayer.Entity;
using DataAccessLayer.Repository.RepositoryInterfaces;
using WebApplication3.DTOs.Responces;
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

        public async Task<AllProductsWithPaginationResponse> GetAllProductAsync(int page, int itemPerPage)
        {
            var expectedProductList = await _prouductRepository.GetAllProductsAsync();

            return new AllProductsWithPaginationResponse
            {
                ProductRespondeList = expectedProductList
                .Skip((page - 1) * itemPerPage)
                .Take(itemPerPage)
                .Select(product => _productMapper.Map(product))
                .ToList(),
                CurrentPage = page,
                Pages = (int)Math.Ceiling(expectedProductList.Count() / (double)itemPerPage)

            };
        }

        public async Task<ProductViewRequestModel> GetProductByIdAsync(int id)
        {
            var expectedProduct = await _prouductRepository.GetProductByIdAsync(id);
            if (expectedProduct == null)
            {
                return null;
            }
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
