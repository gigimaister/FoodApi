using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FoodApi.Models
{
    [Table("SideDishToCart")]
    public class SideDishToCart
    {
        [Key]
        public int Id { get; set; }      
        public int CartId { get; set; }
        public int SideDishId { get; set; }

        [ForeignKey("CartId")]
        public virtual ShoppingCartItem ShoppingCartItem { get; set; }


    }
}
