namespace InventorySystem_Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string SKU { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int ReorderThreshold { get; set; } = 10;
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
