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

        // GET: api/markets/admin TODO: Remove as it was made for test purposes.
        [Authorize(Roles = "Administrator")]
        [Route("admin")]
        public ActionResult<IEnumerable<Market>> GetAllTest()
        {
            try
            {
                return Ok(_marketService.GetAll());
            }
            catch (Exception e)
            {
                return StatusCode(500, $"There was a problem loading all markets. \n{e.Message}");
            }
        }
    }
}