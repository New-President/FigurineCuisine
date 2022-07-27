using System.ComponentModel.DataAnnotations;

namespace FigurineCuisine.Models
{
    public class CartItems
    {
        public int ID { get; set; }
        public int CartID { get; set; }
        public int ProductID { get; set; }

        public int Quantity { get; set; }

        // Navigational Properties
        public Figurine Figurine { get; set; }
    }
}
