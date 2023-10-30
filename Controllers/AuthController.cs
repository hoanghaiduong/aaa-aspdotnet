using Azure.Core;
using BAL.IServices;
using Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace aaa_aspdotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IJwtService jwtService) {
            this._authService = authService;
            this._jwtService = jwtService;
        }
        //[HttpPost("signup")]
        //public Task<Response> SignUp([FromBody] RegisterUserDTO dto)
        //{
        //    //  return new Response(HttpStatusCode.Continue,null);
        //}
        //[HttpPost("signin")]
        //public Task<Response> SignIn([FromBody] RegisterUserDTO dto)
        //{
        //    //  return new Response(HttpStatusCode.Continue,null);

        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RegisterUserDTO dto)
        {
            var payload = new JwtPayload()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = dto.UserName,
                RoleId=Guid.NewGuid().ToString(),
            };
            var accessToken = await _jwtService.GenerateJwtTokenAsync(payload);
            var refreshToken = await _jwtService.RefreshTokenAsync(payload);
            return Ok(new { accessToken,refreshToken });
        }
        [Authorize]
        [HttpPost("refreshToken")]
        public async Task<ActionResult<Response>> RefreshToken([FromBody] RefreshTokenDTO dto)
        {
            try
            {
               
                string accessToken = null;
                string refreshToken = null;
                string authorizationHeader = Request.Headers["Authorization"];
                if (authorizationHeader == null || !authorizationHeader.StartsWith("Bearer "))
                {
                    return Unauthorized();
                }
                var check = await _jwtService.ValidateRefreshTokenAsync(dto.RefreshToken);
                if (!check) return BadRequest(new Response(HttpStatusCode.BadRequest, "RefreshToken is valid"));
                string idToken = authorizationHeader.Substring("Bearer ".Length);
               
                var decodeToken = await _jwtService.DecodeJwtToken(idToken);
               
                if(decodeToken!=null)
                {
                    accessToken = await _jwtService.GenerateJwtTokenAsync(decodeToken);
                    refreshToken = await _jwtService.RefreshTokenAsync(decodeToken);
                }
              
            
                return Ok(new
                {
                    accessToken,
                    refreshToken,
                    decodeToken
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
        public class AccessTokenDTO
        {
            public string AccessToken { get; set; }
        }
        public class RefreshTokenDTO
        {
            public string RefreshToken { get; set; }
        }
        [HttpPost("decodeToken")]
        public async Task<IActionResult> DecodeToken([FromBody] AccessTokenDTO token)
        {
            try
            {
            var decodeToken = await _jwtService.DecodeJwtToken(token.AccessToken);

                return Ok(new
                    {
                        decodeToken
                    });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
