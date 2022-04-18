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
        public ICollection<SideDishToCart> SideDishToCarts { get; set; }

        [NotMapped]
        public ICollection<SideDish> SideDishes { get; set; }

        public bool IsSideDishesEqualToSDTCart(IQueryable<SideDishToCart> sideDishesToCart, ShoppingCartItem shoppingCartItem)
        {
            if (sideDishesToCart is null) return true;
            var sdtCart = new List<SideDishToCart>();
            foreach(var sideDishTocart in shoppingCartItem.SideDishes)
            {
                foreach(var cartSideDish in sideDishesToCart)
                {
                    if(cartSideDish.SideDishId == sideDishTocart.Id)
                    {
                        sdtCart.Add(cartSideDish);
                    }
                }
            }
            var sdtCartList = sideDishesToCart.ToList();
            if (sdtCart.All(sdtCartList.Contains)) return true;
            return false;

        }

    }
}
