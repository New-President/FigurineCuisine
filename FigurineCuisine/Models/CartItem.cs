using System.ComponentModel.DataAnnotations;

namespace FigurineCuisine.Models
{
    public class CartItem
    {
        public int ID { get; set; }
        public string uid { get; set; }
        public int Quantity { get; set; }
        public int FigurineID { get; set; }
        public int CartID { get; set; }
        public Figurine Figurine { get; set; }
        public Cart Cart { get; set; }
    }
}
