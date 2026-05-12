using System.ComponentModel.DataAnnotations;

namespace Testcase_eticaret.API.DTOs
{
    public class CategoryUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "MinimumStockQuantity must be 0 or greater.")]
        public int MinimumStockQuantity { get; set; }
    }
}
