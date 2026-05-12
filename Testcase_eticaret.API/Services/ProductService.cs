using Microsoft.EntityFrameworkCore;
using Testcase_eticaret.API.Data;
using Testcase_eticaret.API.DTOs;
using Testcase_eticaret.API.Models;

namespace Testcase_eticaret.API.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    StockQuantity = p.StockQuantity,
                    IsLive = p.IsLive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return null;

            return new ProductResponseDto
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                StockQuantity = product.StockQuantity,
                IsLive = product.IsLive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public async Task<(ProductResponseDto? Product, string? Error)> CreateAsync(ProductCreateDto dto)
        {
            var category = await _context.Categories.FindAsync(dto.CategoryId);
            if (category == null)
                return (null, "Category not found.");

            var product = new Product
            {
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                StockQuantity = dto.StockQuantity,
                IsLive = dto.StockQuantity >= category.MinimumStockQuantity,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return (new ProductResponseDto
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                CategoryId = product.CategoryId,
                CategoryName = category.Name,
                StockQuantity = product.StockQuantity,
                IsLive = product.IsLive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            }, null);
        }

        public async Task<(ProductResponseDto? Product, string? Error)> UpdateAsync(int id, ProductUpdateDto dto)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return (null, "Product not found.");

            var category = await _context.Categories.FindAsync(dto.CategoryId);
            if (category == null)
                return (null, "Category not found.");

            product.Title = dto.Title;
            product.Description = dto.Description;
            product.CategoryId = dto.CategoryId;
            product.StockQuantity = dto.StockQuantity;
            product.IsLive = dto.StockQuantity >= category.MinimumStockQuantity;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return (new ProductResponseDto
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                CategoryId = product.CategoryId,
                CategoryName = category.Name,
                StockQuantity = product.StockQuantity,
                IsLive = product.IsLive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            }, null);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductResponseDto>> FilterAsync(string? keyword, int? minStock, int? maxStock)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var lowerKeyword = keyword.ToLower();
                query = query.Where(p =>
                    p.Title.ToLower().Contains(lowerKeyword) ||
                    (p.Description != null && p.Description.ToLower().Contains(lowerKeyword)) ||
                    p.Category.Name.ToLower().Contains(lowerKeyword));
            }

            if (minStock.HasValue)
                query = query.Where(p => p.StockQuantity >= minStock.Value);

            if (maxStock.HasValue)
                query = query.Where(p => p.StockQuantity <= maxStock.Value);

            return await query.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                StockQuantity = p.StockQuantity,
                IsLive = p.IsLive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToListAsync();
        }
    }
}
