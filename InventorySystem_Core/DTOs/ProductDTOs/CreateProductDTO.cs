using System.ComponentModel.DataAnnotations;

namespace InventorySystem_Core.DTOs.ProductDTOs
{
    public class CreateProductDTO
    {

        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100,ErrorMessage = "Product Name can't be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "SKU is required.")]
        [StringLength(50, ErrorMessage = "SKU can't be longer than 50 characters.")]
        public string SKU { get; set; } = string.Empty;
        [Range(0.01, 100000.00, ErrorMessage = "Price must be greater than zero")]
        public decimal price { get; set; }
        [Range(0,100000,ErrorMessage ="Stock Quantity must be greater than zero")]
        public int StockQuantity { get; set; }
        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryId { get; set; }
    }
}
