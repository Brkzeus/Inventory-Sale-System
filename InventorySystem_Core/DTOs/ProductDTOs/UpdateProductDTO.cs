using System.ComponentModel.DataAnnotations;

namespace InventorySystem_Core.DTOs.ProductDTOs
{
    public class UpdateProductDTO
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(50)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 100000.00)]
        public decimal Price { get; set; }

        [Range(0, 100000)]
        public int StockQuantity { get; set; }
        [Required(ErrorMessage ="Category ID is required.")]
        public int CategoryId { get; set; }
    }
}
