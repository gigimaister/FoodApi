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
        // For Main Course
        public int MainCourseToProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public virtual ICollection<SideDishToCart> SideDishToCarts { get; set; }

        [NotMapped]
        public virtual ICollection<SideDish> SideDishes { get; set; }

        [NotMapped]
        public virtual ICollection<PaidSideDish> PaidSideDishes { get; set; }

        /// <summary>
        /// Check If SideDishe List Is Null Or Empty
        /// </summary>
        /// <returns>
        /// True If Empty List</returns>
        public bool IsSideDishesEmpty()
        {
            if (SideDishes != null)
            {
                if (SideDishes.Count > 0)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Check If Paid SideDishe List Is Null Or Empty
        /// </summary>
        /// <returns>
        /// True If Empty List</returns>
        public bool IsPaidSideDishesEmpty()
        {
            if (PaidSideDishes != null)
            {
                if (PaidSideDishes.Count > 0)
                {
                    return false;
                }
            }
            return true;

        }
        /// <summary>
        /// Check If The Post Shopping Cart Item Is The Same As The One In DB If Any, Including SD And PSD.
        /// </summary>
        /// <returns>True If User Post A New One</returns>
        public bool IsSCartNew(List<ShoppingCartItem> shoppingCartItems)
        {
            var shoppingCart = shoppingCartItems.FirstOrDefault();

            // If We Have Another Product With The Same Id We Will Check If It Has SideDish List
            if (shoppingCart != null)
            {
                // Only SideDish No Paid Dish
                if (!IsSideDishesEmpty() && IsPaidSideDishesEmpty())
                {
                    foreach (var sCartItem in shoppingCartItems)
                    {
                        var sCartItemIds = sCartItem.SideDishToCarts.Where(x => !x.IsChargeExtra).Select(x => x.SideDishId).OrderBy(e => e).ToList();
                        var shoppingCartItemIds = SideDishes.Select(x => x.Id).OrderBy(e => e).ToList();

                        bool isEqual = Enumerable.SequenceEqual(sCartItemIds, shoppingCartItemIds);
                        // If We Got A Match
                        if (isEqual)
                        {
                            return false;
                        }
                        // No Match
                        else
                        {
                            return true;
                        }
                    }
                }
                // Only PaidDish No SideDish
                if (IsSideDishesEmpty() && !IsPaidSideDishesEmpty())
                {
                    foreach (var sCartItem in shoppingCartItems)
                    {
                        var sCartItemIds = sCartItem.SideDishToCarts.Where(x => x.IsChargeExtra).Select(x => x.SideDishId).OrderBy(e => e).ToList();
                        var shoppingCartItemIds = PaidSideDishes.Select(x => x.Id).OrderBy(e => e).ToList();

                        bool isEqual = Enumerable.SequenceEqual(sCartItemIds, shoppingCartItemIds);
                        // If We Got A Match
                        if (isEqual)
                        {
                            return false;
                        }
                        // No Match
                        else
                        {
                            return true;
                        }
                    }
                }
                // Both
                if (!IsSideDishesEmpty() && !IsPaidSideDishesEmpty())
                {
                    // Check If Both Of The Are Equal
                    // SideDish
                    foreach (var sCartItem in shoppingCartItems)
                    {
                        // SideDish
                        var sCartItemIds = sCartItem.SideDishToCarts.Where(x => !x.IsChargeExtra).Select(x => x.SideDishId).OrderBy(e => e).ToList();
                        var shoppingCartItemIds = SideDishes.Select(x => x.Id).OrderBy(e => e).ToList();
                        bool isEqual = Enumerable.SequenceEqual(sCartItemIds, shoppingCartItemIds);

                        // Paid SideDish
                        var psCartItemIds = sCartItem.SideDishToCarts.Where(x => x.IsChargeExtra).Select(x => x.SideDishId).OrderBy(e => e).ToList();
                        var pshoppingCartItemIds = PaidSideDishes.Select(x => x.Id).OrderBy(e => e).ToList();
                        bool pisEqual = Enumerable.SequenceEqual(psCartItemIds, pshoppingCartItemIds);

                        if (isEqual && pisEqual)
                        {
                            return false;
                        }

                    }

                }
            }

            return true;
        }
    }
}
