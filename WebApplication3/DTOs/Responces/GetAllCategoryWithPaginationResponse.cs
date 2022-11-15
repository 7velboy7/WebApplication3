using WebApplication3.UserViewRequestsModel;

namespace WebApplication3.DTOs.Responces
{
    public class AllCategoriesWithPaginationResponse
    {
        public List<CategoryViewRequestModel> CategoryResponceList { get; set; }
        
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        
    }
}
