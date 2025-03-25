using System.ComponentModel.DataAnnotations.Schema;

namespace Mini_Amazon_Clone.Models
{
    public class Order
    {

        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public string Status{ get; set; }


        // Navigation property ( One to Many relationship ) One user can have multiple orders 
        public int UserID { get; set; } // FK to refrence the user
        public User User { get; set; }


        // Navigation Properties ( Many to Many relationship ) an order can have multiple products and a product can belong to multiple orders
        public List<OrderItem> OrderItems { get; set; }



    }
}
