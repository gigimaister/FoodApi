﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FoodApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string ImageUrl { get; set; }        
        public double Price { get; set; }
        public bool IsPopularProduct { get; set; }
        public bool IsProductSelectable { get; set; }
        public bool HasPaidSideDish { get; set; }
        public int CategoryId { get; set; }
        public bool IsMeatSelect { get; set; }
        public bool IsFishSelect { get; set; }
        public bool IsVegSelect { get; set; }
        public int MaxMeatSelect { get; set; }
        public int MaxFishSelect { get; set; }
        public int MaxVegSelect { get; set; }
        public bool HasPaidMainCourse { get; set; }

        // For Main Course
        public int MainCourseToProductId { get; set; }
        public virtual MainCourseToProduct MainCoursetoProduct { get; set; }

        [ForeignKey("ProductId")]
        public virtual ICollection<MainCourseToProduct> MainCourseToProduct { get;}

        [NotMapped]
        public virtual ICollection<SideDish> SideDishList { get; set; }
       
        [NotMapped]
        //[JsonIgnore]
        public byte[] ImageArray { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
