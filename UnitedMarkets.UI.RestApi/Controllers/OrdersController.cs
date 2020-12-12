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
    }
}