using System.ComponentModel.DataAnnotations;

namespace Testcase_eticaret.API.DTOs
{
    public class ProductCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "StockQuantity must be 0 or greater.")]
        public int StockQuantity { get; set; }
    }
}
