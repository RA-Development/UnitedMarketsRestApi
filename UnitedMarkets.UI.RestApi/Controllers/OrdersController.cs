using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.UI.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IService<Order> _orderService;

        public OrdersController(IService<Order> orderService)
        {
            _orderService = orderService;
        }

        // GET
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAll()
        {
            try
            {
                return Ok(_orderService.GetAll());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST
        [HttpPost]
        public ActionResult Post([FromBody] Order order)
        {
            try
            {
                return Ok(_orderService.Create(order));
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Order order)
        {
            if (id != order.Id)
            {
                return BadRequest("Order id must match.");
            }

            try
            {
                return Ok(_orderService.Update(order));
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}