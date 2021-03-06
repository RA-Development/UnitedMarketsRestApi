﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UnitedMarkets.UI.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IService<Product> _productService;

        public ProductsController(IService<Product> productService)
        {
            _productService = productService;
        }


        // GET: api/<ProductsController>
        [HttpGet]
        public ActionResult<FilteredList<Product>> Get([FromQuery] Filter filter)
        {
            try
            {
                if (_productService.GetAll(filter) != null)
                    return Ok(_productService.GetAll(filter));
                else
                    return BadRequest("filter error");
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}