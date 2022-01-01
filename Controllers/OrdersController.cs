using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {

        private readonly IDutchRepository _repository;
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<StoreUser> _userManager;

        public OrdersController(IDutchRepository repository, ILogger<ProductsController> logger, IMapper mapper, UserManager<StoreUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET all orders - can pass "includeItems" as URL param - true by default
        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var username = User.Identity.Name;
                var results = _repository.GetAllOrdersByUser(username, includeItems);
                return Ok(_mapper.Map<IEnumerable<OrderViewModel>>(results));
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to fetch orders: {ex}");
                return BadRequest("Failed to fetch orders");
            }
        }

        // GET order by ID
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = _repository.GetOrderById(User.Identity.Name, id);

                if (order != null)
                {
                    // Return order with 200 status - map to viewModel
                    return Ok(_mapper.Map<Order, OrderViewModel>(order));
                } else
                {
                    return NotFound();
                }
        
            }
            catch (Exception ex)
            {

                
                return BadRequest($"Failed to fetch order {id}: {ex}");
            }
        }

        // POST new order
        [HttpPost]
        public async Task<IActionResult>  Post([FromBody]OrderViewModel model)
        {
            // Add to db
            try
            {
                // Check model is valid
                if (ModelState.IsValid)
                {
                    // Convert from viewModel back to order
                    var newOrder = _mapper.Map<OrderViewModel, Order>(model);

                    // If no order date entered, set as now
                    if(newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    // Set the user of the new order to current user
                    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    newOrder.User = currentUser;

                    // Add to db via repository
                    _repository.AddEntity(newOrder);

                    // If saved successfully...
                    if (_repository.SaveAll())
                    {
                        // Return new order (map to viewModel) - 201 status
                        return Created($"/api/orders/{newOrder.Id}", _mapper.Map<Order, OrderViewModel>(newOrder));
                    }
                }

                // If not valid, return validation errors
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to add order: {ex}");
            }

            return BadRequest("Failed to add order");
        }
    }
}
