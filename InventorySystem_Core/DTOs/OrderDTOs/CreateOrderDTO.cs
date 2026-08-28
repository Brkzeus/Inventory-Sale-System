using System.ComponentModel.DataAnnotations;

namespace InventorySystem_Core.DTOs.OrderDTOs
{
    public class CreateOrderDTO
    {
        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Order must contain at least one item.")]
        [MinLength(1, ErrorMessage = "An order must contain at least one item.")]
        public List<OrderItemDTO> Items { get; set; } = new();
    }
}

