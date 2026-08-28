using System.ComponentModel.DataAnnotations;

namespace InventorySystem_Core.DTOs.CategoryDTOs
{
    public class UpdateCategoryDTO
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string Description { get; set; } = string.Empty;
    }
}
