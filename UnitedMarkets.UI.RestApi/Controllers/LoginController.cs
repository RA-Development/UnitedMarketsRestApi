using System;
using Microsoft.AspNetCore.Mvc;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.Entities.AuthenticationModels;

namespace UnitedMarkets.UI.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/login
        [HttpPost]
        public IActionResult Login([FromBody] LoginInputModel loginInputModel)
        {
            try
            {
                var token = _userService.ValidateUser(loginInputModel);

                return Ok(new
                {
                    loginInputModel.Username,
                    token
                });
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
                //return BadRequest(e.Message);
            }
        }
    }
}