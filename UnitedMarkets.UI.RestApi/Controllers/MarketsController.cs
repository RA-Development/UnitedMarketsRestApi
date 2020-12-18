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
    public class MarketsController : ControllerBase
    {
        private readonly IService<Market> _marketService;

        public MarketsController(IService<Market> marketService)
        {
            _marketService = marketService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Market>> GetAll()
        {
            try
            {
                return Ok(_marketService.GetAll());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}