using System.ComponentModel.DataAnnotations;

namespace FigurineCuisine.Models
{
    public class CartItem
    {
        public int ID { get; set; }
        public string CartID { get; set; }

        public int Quantity { get; set; }

        // Navigational Properties
        public int FigurineID { get; set; }
    }
}
