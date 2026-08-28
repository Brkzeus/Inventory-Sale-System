using System.ComponentModel.DataAnnotations;

namespace InventorySystem_Core.DTOs.OrderDTOs
{
    public class UpdateOrderStatusDTO
    {
        [Required]
        [RegularExpression("^(Pending|Processing|Shipped|Delivered|Cancelled)$",ErrorMessage = "Invalid status value.")]
        public string Status { get; set; } = string.Empty;
    }
}
