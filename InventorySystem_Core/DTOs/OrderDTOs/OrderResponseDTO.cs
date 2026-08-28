namespace InventorySystem_Core.DTOs.OrderDTOs
{
    public class OrderResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<OrderItemResponseDTO> Items { get; set; } = new();
    }
}
