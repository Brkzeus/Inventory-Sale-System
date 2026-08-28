using System.ComponentModel.DataAnnotations;

namespace InventorySystem_Core.DTOs.CategoryDTOs
{
    public class CreateCategoryDTO
    {

        [Required(ErrorMessage = "Category Name is required.")]
        [StringLength(100, ErrorMessage = "Category Name can't be longer than 100 chaarcters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category Description is required.")]
        [StringLength(1000, ErrorMessage = "Category Description can't be longer than 1000 chaarcters")]

        public string Description { get; set; } = string.Empty;

    }
}
