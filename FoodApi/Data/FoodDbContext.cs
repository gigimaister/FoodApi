﻿using FoodApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodApi.Data
{
    public class FoodDbContext : DbContext
    {
        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<MainDishes> MainDishess { get; set; }
        public DbSet<SideDish> SideDishes { get; set; }
        public DbSet<SideDishToCart> SideDishToCarts { get; set; }
        public DbSet<MainCourseProductDishes> MainCourseProductDishes { get; set; }
        public DbSet<MainCourseToProduct> mainCourseToProducts { get; set; }
    }
}
