﻿using aaa_aspdotnet.src.BAL.IServices;
using aaa_aspdotnet.src.BAL.Services;
using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.Helpers;
using aaa_aspdotnet.src.Common.pagination;
using aaa_aspdotnet.src.DAL.Entities;
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

            try
            {
                var user = await _userService.CreateOrUpdateUserWithHelper(dto);
                return Ok(new Response(HttpStatusCode.OK, "Tạo người dùng mới thành công !", user));
            }
            catch (SqlException sqlEx)
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, sqlEx.Message));

            }

            catch (Exception ex)
            {

                return BadRequest(new Response(HttpStatusCode.BadRequest, ex.Message));
            }

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
                return Ok(new Response(HttpStatusCode.OK, "Cập nhật người dùng mới thành công !", isUpdate));
            }
            catch (SqlException sqlEx)
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, sqlEx.Message));

            }

            catch (Exception ex)
            {

                return BadRequest(new Response(HttpStatusCode.BadRequest, ex.Message));
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
                return Ok(new Response(HttpStatusCode.OK, "Cập nhật người dùng mới thành công !", isUpdate));
            }
            catch (SqlException sqlEx)
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, sqlEx.Message));

            }

            catch (Exception ex)
            {

                return BadRequest(new Response(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [HttpGet("get-users")]

        public async Task<ActionResult<Response>> GetUsers([FromQuery] PaginationFilterDto filter)
        {
            var users=  _userService.GetUsers(filter);
            var metadata = new
            {
                users.TotalCount,
                users.PageSize,
                users.CurrentPage,
                users.TotalPages,
                users.HasNext,
                users.HasPrevious
            };
            return Ok(new Response(HttpStatusCode.OK,"Lấy danh sách người dùng thành công",new
            {
                users,
                metadata
            }));
            // Sử dụng các thuộc tính của filter để thực hiện truy vấn với procedure
            // Sau đó, trả về kết quả như đã thực hiện trước đó
        }

        [HttpGet("get-user-by-Id")]

        public async Task<ActionResult<Response>> GetUserById([FromQuery] string id)
        {
            var user=await _userService.GetUserById(id);
             if(user==null)
            {
                return NotFound(new Response(HttpStatusCode.NotFound,"User not found"));
            }    
             return Ok(user);
            // Sử dụng các thuộc tính của filter để thực hiện truy vấn với procedure
            // Sau đó, trả về kết quả như đã thực hiện trước đó
        }
    }
}
