using DataAccessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.RepositoryInterfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategorytByIdAsync(int productId);
        Task RemoveCategoryByIdAsync(int productId);
        Task AddCategoryAsync(Category product);
        Task UpdateCategoryAsync(Category product);
    }
}
