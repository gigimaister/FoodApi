using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using FoodApi.Models;
using FoodApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System;

namespace FoodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartItemsController : ControllerBase
    {
        private FoodDbContext _dbContext;
        public ShoppingCartItemsController(FoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/ShoppingCartItems
        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {
            var sCartItems = _dbContext.ShoppingCartItems.Where(s => s.CustomerId == userId);
            var sDishes = _dbContext.SideDishes.Select(x => x).ToList();

            if (sCartItems == null)
            {
                return NotFound();
            }
            var shoppingCartItems = from s in _dbContext.ShoppingCartItems.Where(s => s.CustomerId == userId)
                                    join p in _dbContext.Products on s.ProductId equals p.Id


                                    select new
                                    {
                                        Id = s.Id,
                                        Price = s.Price,
                                        TotalAmount = s.TotalAmount,
                                        Qty = s.Qty,
                                        ProductName = p.Name,
                                        SideDishToCarts = s.SideDishToCarts,
                                        Product = s.Product
                                        //SideDishes = s.ReturnSideDishFromSDTC(sDishes)

                                    };

            return Ok(shoppingCartItems);
        }

        // GET: api/ShoppingCartItems/SubTotal/5

        [HttpGet("[action]/{userId}")]
        public IActionResult SubTotal(int userId)
        {
            var subTotal = (from cart in _dbContext.ShoppingCartItems
                            where cart.CustomerId == userId
                            select cart.TotalAmount).Sum();
            return Ok(new { SubTotal = subTotal });
        }


        // GET: api/ShoppingCartItems/TotalItems/5
        [HttpGet("[action]/{userId}")]
        public IActionResult TotalItems(int userId)
        {
            var cartItems = (from cart in _dbContext.ShoppingCartItems
                             where cart.CustomerId == userId
                             select cart.Qty).Sum();
            return Ok(new { TotalItems = cartItems });
        }

        // POST: api/ShoppingCartItems
        [HttpPost]
        public IActionResult Post([FromBody] ShoppingCartItem shoppingCartItem)
        {
            var shoppingCartList = _dbContext.ShoppingCartItems
                .Where(s => s.ProductId == shoppingCartItem.ProductId && s.CustomerId == shoppingCartItem.CustomerId)
                .Include(s => s.SideDishToCarts).Select(x=>x)
                .Include(s => s.Product).ToList();

            var shoppingCart = shoppingCartList.FirstOrDefault();
         
            // If We Have Another Product With The Same Id We Will Check If It Has SideDish List
            if(shoppingCart != null)
            {
                // If  Product Doesn't Contain SideDish List
                if (!shoppingCart.Product.IsProductSelectable)
                {
                    // If What We Post To Shopping Cart SideDish Is Null Or We Have The Same SideList Just Multiply The Qty               
                    shoppingCart.Qty += shoppingCartItem.Qty;
                    shoppingCart.TotalAmount = shoppingCart.Price * shoppingCart.Qty;
                }
                // If We Have SideDish List We Will Check If We Have A Match With The New Post One
                else
                {
                    bool isEqual = false;

                    foreach (var sCartItem in shoppingCartList)
                    {
                        var sCartItemIds = sCartItem.SideDishToCarts.Select(x => x.SideDishId).OrderBy(e => e).ToList();
                        var shoppingCartItemIds = shoppingCartItem.SideDishes.Select(x => x.Id).OrderBy(e => e).ToList();

                        isEqual = Enumerable.SequenceEqual(sCartItemIds, shoppingCartItemIds);
                        // If We Got A Match
                        if (isEqual)
                        {
                            // If What We Post To Shopping Cart SideDish Is Null Or We Have The Same SideList Just Multiply The Qty               
                            sCartItem.Qty += shoppingCartItem.Qty;
                            sCartItem.TotalAmount = sCartItem.Price * sCartItem.Qty;
                            break;
                        }
                    
                    }
                    if (!isEqual)
                    {
                        var sCart = new ShoppingCartItem()
                        {
                            CustomerId = shoppingCartItem.CustomerId,
                            ProductId = shoppingCartItem.ProductId,
                            Price = shoppingCartItem.Price,
                            Qty = shoppingCartItem.Qty,
                            TotalAmount = shoppingCartItem.TotalAmount
                        };
                        _dbContext.ShoppingCartItems.Add(sCart);
                        _dbContext.SaveChanges();

                        // If We Have A Product That Has Side Dishes
                        if (shoppingCartItem.SideDishes != null)
                        {
                            foreach (var sideDish in shoppingCartItem.SideDishes)
                            {
                                var sDToCart = new SideDishToCart()
                                {
                                    SideDishId = sideDish.Id,
                                    CartId = sCart.Id
                                };
                                _dbContext.SideDishToCarts.Add(sDToCart);
                            }
                        }
                    }
                    
                }
            } 
            // No Other Product Like That So We Create A New One
            else
            {
                var sCart = new ShoppingCartItem()
                {
                    CustomerId = shoppingCartItem.CustomerId,
                    ProductId = shoppingCartItem.ProductId,
                    Price = shoppingCartItem.Price,
                    Qty = shoppingCartItem.Qty,
                    TotalAmount = shoppingCartItem.TotalAmount
                };
                _dbContext.ShoppingCartItems.Add(sCart);
                _dbContext.SaveChanges();

                // If We Have A Product That Has Side Dishes
                if (shoppingCartItem.SideDishes != null)
                {
                    foreach (var sideDish in shoppingCartItem.SideDishes)
                    {
                        var sDToCart = new SideDishToCart()
                        {
                            SideDishId = sideDish.Id,
                            CartId = sCart.Id,
                            IsChargeExtra = false

                        };
                        _dbContext.SideDishToCarts.Add(sDToCart);
                    }
                }
                // If We Have A Product That Has !! PAID !! Side Dishes
                if (shoppingCartItem.PaidSideDishes != null)
                {
                    foreach (var paidSideDish in shoppingCartItem.SideDishes)
                    {
                        var sDToCart = new SideDishToCart()
                        {
                            SideDishId = paidSideDish.Id,
                            CartId = sCart.Id,
                            IsChargeExtra = true
                        };
                        _dbContext.SideDishToCarts.Add(sDToCart);
                    }
                }

            }
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{userId}")]
        public IActionResult Put(int userId, ShoppingCartItem sCartItem)
        {
            try
            {
                var shopingCartItem = _dbContext.ShoppingCartItems.Where(s => s.CustomerId == userId && s.ProductId == sCartItem.ProductId).FirstOrDefault();
                if (shopingCartItem != null)
                {
                    shopingCartItem.Qty = sCartItem.Qty;
                    shopingCartItem.TotalAmount = shopingCartItem.Price * shopingCartItem.Qty;
                    // If We Have A Product That Has Side Dishes
                    if (sCartItem.SideDishes != null)
                    {
                        sCartItem.SideDishToCarts = new List<SideDishToCart>();

                        foreach (var sideDish in sCartItem.SideDishes)
                        {                           
                            var sDToCart = new SideDishToCart()
                            {
                                SideDishId = sideDish.Id,
                                CartId = shopingCartItem.Id
                            };
                            sCartItem.SideDishToCarts.Add(sDToCart);
                        }

                        // Delete Old SideDishes
                        _dbContext.SideDishToCarts.RemoveRange(shopingCartItem.SideDishToCarts);
                        foreach (var sCartSdish in sCartItem.SideDishToCarts)
                        {
                            _dbContext.SideDishToCarts.Add(sCartSdish);
                        }
                    }
                    _dbContext.ShoppingCartItems.Update(shopingCartItem);
                   
                    _dbContext.SaveChanges();
                }
                return Accepted();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{userId}")]
        public IActionResult Delete(int userId)
        {
            var shoppingCart = _dbContext.ShoppingCartItems.Where(s => s.CustomerId == userId);
            var sDishToCart = _dbContext.SideDishToCarts.Select(x => x);

            // Select Only Matching SideDish To Cart Item
            var sDishToCartUpdated = from first in sDishToCart
                                       join second in shoppingCart
                                       on first.CartId equals second.Id
                                       select first;

            _dbContext.ShoppingCartItems.RemoveRange(shoppingCart);
            _dbContext.SideDishToCarts.RemoveRange(sDishToCart);
            _dbContext.SaveChanges();
            return Ok();

        }

        // DELETE: api/ShoppingCartItems/DeleteProductFromCart/5/2
        [HttpDelete("[action]/{userId}/{productId}")]
        public IActionResult DeleteProductFromCart(int userId, int productId)
        {
            var shoppingCart = _dbContext.ShoppingCartItems.FirstOrDefault(s => s.CustomerId == userId && s.Id == productId);
            
            _dbContext.ShoppingCartItems.RemoveRange(shoppingCart);
            _dbContext.SideDishToCarts.RemoveRange(shoppingCart.SideDishToCarts);
            _dbContext.SaveChanges();
            return Ok();

        }
    }
}
