using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _ctx;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        // GET all orders
        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
         
            try
            {
                _logger.LogInformation("GetAllOrders");

                if(includeItems)
                {
                    return _ctx.Orders
                        .Include(order => order.Items)
                        .ThenInclude(item => item.Product)
                        .ToList();
                } else
                {
                    return _ctx.Orders
                        .ToList();
                }
              
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to fetch all orders: {ex}");
                return null;
            }
        }


        // GET order by ID
        public Order GetOrderById(string username, int id)
        {
  

            try
            {
                _logger.LogInformation("GetOrderById");
                return _ctx.Orders
                    .Include(order => order.Items)
                    .ThenInclude(item => item.Product)
                    .Where(order => order.Id == id && order.User.UserName == username )
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to fetch order {id}: {ex}");
                return null;
            }
        }

        // GET all orders by user
        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            if (includeItems)
            {
                return _ctx.Orders
                    .Where(order => order.User.UserName == username)
                    .Include(order => order.Items)
                    .ThenInclude(item => item.Product)
                    .ToList();
            }
            else
            {
                return _ctx.Orders
                    .ToList();
            }
        }


        // GET all products
        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("GetAllProducts");
                return _ctx.Products.OrderBy(p => p.Title).ToList();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to fetch all products: {ex}");
                return null;
            }
   
        }

        // GET products by category
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
       

            try
            {
                _logger.LogInformation("GetProductsByCategory");
                return _ctx.Products.Where(p => p.Category == category).ToList();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to fetch products by category: {ex}");
                return null;
            }
        }


        // Save DB changes
        public bool SaveAll()
        {
    
            try
            {
                _logger.LogInformation("SaveAll");
                return _ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to save changes: {ex}");
                return false;
            }
        }


        // POST add data to DB
        public void AddEntity(object model)
        {

            try
            {
                _logger.LogInformation("AddEntity");
                _ctx.Add(model);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to add entity: {ex}");
            }
        }

       
    }
}
