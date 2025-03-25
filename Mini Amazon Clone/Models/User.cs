namespace Mini_Amazon_Clone.Models
{
    public class User
    {
        public int UserID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }


        // Navigation property ( One to Many relationship ) One user can have multiple orders 
        public List<Order> Orders { get; set; } // One user can have multiple orders


        // Navigation Property ( One to Many relationship ) one admin can create/update multiple products
        public List<Product> Products { get; set; }             


    }
}
