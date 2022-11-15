using WebApplication3.UserViewRequestsModel;

namespace WebApplication3.DTOs.Responces
{
    public class AllProductsWithPaginationResponse
    {
        public List<ProductViewRequestModel> ProductRespondeList { get; set; }

        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
