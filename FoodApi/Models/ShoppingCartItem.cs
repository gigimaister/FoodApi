using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FoodApi.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public double Price { get; set; }     
        public int Qty { get; set; }
        public double TotalAmount { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public virtual ICollection<SideDishToCart> SideDishToCarts { get; set; }

        [NotMapped]
        public virtual ICollection<SideDish> SideDishes { get; set; }
             
    }

}
