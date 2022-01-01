﻿using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext ctx, IWebHostEnvironment env, UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _env = env;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            // Ensure database exists
            _ctx.Database.EnsureCreated();

            // See if user exists with this email
            StoreUser user = await _userManager.FindByEmailAsync("kai@dutchtreat.com");

            // If doesn't exist, need to create it
            if(user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Kai",
                    LastName = "Sells",
                    Email = "kai@dutchtreat.com",
                    UserName = "kai@dutchtreat.com"
                };

                var result = await _userManager.CreateAsync(user, "Password1!");

                // Throw error if not successfully created
                if(result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
            }

            // "!_ctx.Products.Any()" returns true if no products in db - if so, need to create sample data
            if (!_ctx.Products.Any())
            {
                var filePath = Path.Combine(_env.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);

                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                _ctx.Products.AddRange(products);

                var order = new Order()
                {
                    OrderDate = DateTime.Today,
                    OrderNumber = "10000",
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    }
                };

                order.User = user;

                _ctx.Orders.Add(order);

                _ctx.SaveChanges();
            }
        }
    }
}
