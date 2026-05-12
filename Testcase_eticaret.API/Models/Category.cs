using System.ComponentModel.DataAnnotations;

namespace Testcase_eticaret.API.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "MinimumStockQuantity cannot be negative.")]
        public int MinimumStockQuantity { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
