using Testcase_eticaret.API.DTOs;

namespace Testcase_eticaret.API.Services
{
    public interface IProductService
    {
        Task<List<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto?> GetByIdAsync(int id);
        Task<(ProductResponseDto? Product, string? Error)> CreateAsync(ProductCreateDto dto);
        Task<(ProductResponseDto? Product, string? Error)> UpdateAsync(int id, ProductUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<ProductResponseDto>> FilterAsync(string? keyword, int? minStock, int? maxStock);
    }
}
