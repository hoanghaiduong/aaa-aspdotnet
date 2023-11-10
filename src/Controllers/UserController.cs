using aaa_aspdotnet.Filters;
using aaa_aspdotnet.src.BAL.IServices;
using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace aaa_aspdotnet.src.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public IAuthService _authService;
        private readonly IJwtService _jwtService;

        public UserController(IUserService userService,IAuthService authService,IJwtService jwtService)
        {
            _userService = userService;
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Response>> CreateUser([FromBody] CreateOrUpdateUserDTO dto)
        {
            var creating = await _userService.CreateOrUpdateUserWithHelper(dto);
            if(creating.Status==HttpStatusCode.BadRequest.ToString())
            {
                return BadRequest(creating);
            }
            return Ok(creating);
        }
        [Authorize]
        [HttpPost("update")]
        public async Task<ActionResult<Response>> update([FromBody] UpdateUserDTO dto)
        {
            try
            {
                
                string token= await _authService.GetToken(Request);
              
                var decodeToken = await _jwtService.DecodeJwtToken(token);
                if(decodeToken==null)
                {
                    return BadRequest("Token is valid");
                }
                var isUpdate = await _userService.CreateOrUpdateUserWithHelper(new CreateOrUpdateUserDTO { 
                  PhoneNumber = dto.PhoneNumber,
                  Email = dto.Email,
                  Gender = dto.Gender,
                  Avatar = dto.Avatar,
               },decodeToken.Id);
                if (isUpdate.Status == HttpStatusCode.OK.ToString())
                {
                    return Ok(isUpdate);
                }
                else
                {
                    return BadRequest(isUpdate.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
       public class UpdateUserByAdminDTO
        {
            public bool IsActived { get; set; }
            public bool IsDeleted { get; set; }
        }
        [Authorize]
        [HttpPut("update-by-admin")]
        public async Task<ActionResult<Response>> UpdateUserByAdmin([FromQuery] string id, [FromBody] UpdateUserByAdminDTO dto)
        {

            try
            {

                string token = await _authService.GetToken(Request);
                
                var check = await _authService.CheckRole(token);
                if(!check)
                {
                    return Forbid("Bạn không có quyền truy cập");
                }    
                var isUpdate= await _userService.CreateOrUpdateUserWithHelper(new CreateOrUpdateUserDTO
                {
                   IsActived=dto.IsActived,
                   IsDeleted=dto.IsDeleted
                }, id);
                if(isUpdate.Status==HttpStatusCode.OK.ToString())
                {
                    return Ok(isUpdate);
                }    
                else
                {
                    return BadRequest(isUpdate.Message);
                }    
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-users")]

        public async Task<ActionResult<Response>> GetUsers([FromQuery] UserFilterDto filter)
        {
            return Ok(filter);
            // Sử dụng các thuộc tính của filter để thực hiện truy vấn với procedure
            // Sau đó, trả về kết quả như đã thực hiện trước đó
        }
    }
}
