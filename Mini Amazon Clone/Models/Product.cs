using System.ComponentModel.DataAnnotations.Schema;

namespace Mini_Amazon_Clone.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        // Navigation Property ( One to many relationship ) one admin can create/update multiple products

        public int CreatedBy { get; set; } // This is FK refrence the creator ( Admin ) 

        public User Admin { get; set; }

        // Navigation Properties ( Many to Many relationship ) an order can have multiple products and a product can belong to multiple orders
        public List<OrderItem> OrderItems { get; set; }



    }
}
