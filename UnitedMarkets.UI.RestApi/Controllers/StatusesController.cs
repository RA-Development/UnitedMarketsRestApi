using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.UI.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : Controller
    {
        private IService<OrderStatus> _service;

        public StatusesController(IService<OrderStatus> statusService)
        {
            _service = statusService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Market>> GetAll()
        {
            try
            {
                return Ok(_service.GetAll());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}