using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("/api/orders/{orderId}/items")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemsController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IMapper _mapper;

        public OrderItemsController(IDutchRepository repository, ILogger<OrderItemsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        // GET items for specific order by order ID
        [HttpGet]
        public IActionResult Get(int orderId)
        {
            var order = _repository.GetOrderById(User.Identity.Name, orderId);
            if (order != null)
            {
                return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
            }
            else
            {
                return NotFound();
            }
        }

        // GET specific item from an order - by order ID and item ID - /api/orders/orderId/items/id
        [HttpGet("{id}")]
        public IActionResult Get(int orderId, int id)
        {
            // Find order by ID
            var order = _repository.GetOrderById(User.Identity.Name, orderId);

            // If order found,
            if (order != null)
            {
                // Get the specific order item by ID
                var item = order.Items.Where(item => item.Id == id).FirstOrDefault();

                // If item is found, return it (map to viewmodel first)
                if(item != null)
                {
                    return Ok(_mapper.Map<OrderItem, OrderItemViewModel>(item));

                }
            
            }

            return NotFound();
        }
    }
}
