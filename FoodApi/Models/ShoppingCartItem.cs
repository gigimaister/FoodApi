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

        [NotMapped]
        public ICollection<SideDish> SideDishes { get; set; }

        public bool IsSideDishesEqualToSDTCart(IQueryable<SideDishToCart> sideDishesToCart, ShoppingCartItem shoppingCartItem)
        {
            if (sideDishesToCart is null) return false;
            // Select Only Matching SideDish To Cart Item
            var sDishToCartUpdated = from first in sideDishesToCart
                                     join second in shoppingCartItem.SideDishes
                                     on first.CartId equals second.Id
                                     select first;
            if (sDishToCartUpdated != sideDishesToCart) return false;
            return true;

        }

    }
}
