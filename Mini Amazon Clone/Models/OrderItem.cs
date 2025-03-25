using System.ComponentModel.DataAnnotations.Schema;

namespace Mini_Amazon_Clone.Models
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }

        public int OrderID { get; set; }

        public int ProductID { get; set; }


        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Navigation Properties ( Many to Many relationship ) an order can have multiple products and a product can belong to multiple orders

        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
