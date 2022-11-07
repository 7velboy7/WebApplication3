using DataAccessLayer.Entity;
using WebApplication3.Mappers.MapperInterface;
using WebApplication3.UserViewRequestsModel;

namespace WebApplication3.Mappers.MapperImplementation
{
    public class CategoryMapper : IMapper<Category, CategoryViewRequestModel>
    {
        /// <inheritdoc/>
        public CategoryViewRequestModel Map(Category toMap)
        {
            CategoryViewRequestModel category = new CategoryViewRequestModel();
            category.Name = toMap.Name;
            category.ParentCategory = toMap.ParentCategoryId;

            return category;
        }
    }
}
