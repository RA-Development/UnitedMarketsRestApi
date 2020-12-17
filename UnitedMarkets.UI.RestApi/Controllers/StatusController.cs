using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.UI.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : Controller
    {
        private readonly IService<Status> _service;

        public StatusController(IService<Status> statusService)
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