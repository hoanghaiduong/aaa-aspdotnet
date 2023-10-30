using BAL.IServices;
using BAL.Services;
using Common.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace aaa_aspdotnet.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        public async Task<Response> CreateUser([FromBody] CreateUserDTO dto)
        {
             return await _userService.CreateUser(dto);
        }

        [HttpGet("get")]
        public async Task<IActionResult> get()
        {

            return Ok(new
            {

            });
        }
    }
    }
