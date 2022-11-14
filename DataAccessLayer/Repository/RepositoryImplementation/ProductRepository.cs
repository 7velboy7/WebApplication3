using DataAccessLayer.Contexts;
using DataAccessLayer.Entity;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.RepositoryImplementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly Context _context;

        public ProductRepository(Context context)
        {
            _context = context;
        }

       
        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(products => products.CategoryId).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var expectedProduct = await _context.Products.FindAsync(productId);

            if (expectedProduct == null)
            {
                return null;
            }
            return expectedProduct;
        }

        public async Task RemoveProductByIdAsync(int productId)
        {
            var product = await GetProductByIdAsync(productId);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
