using aaa_aspdotnet.src.BAL.IServices;
using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.Helpers;
using aaa_aspdotnet.src.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace aaa_aspdotnet.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly AppDbContext _context;
        
        public AuthController(IAuthService authService, IJwtService jwtService, AppDbContext context, IUserService userService)
        {
            _authService = authService;
            _jwtService = jwtService;
            _context = context;
            _userService = userService;
        }
    
        [HttpPost("signup")]
        public async Task<ActionResult<Response>> SignUp([FromBody] RegisterUserDTO dto)
        {
            try
            {
                
                var parameters = new Dictionary<string, object>
                {
                        { "@Username", dto.UserName },
                        { "@Password", dto.Password },

                };

                var result = await _userService.CreateOrUpdateUserWithHelper(new CreateOrUpdateUserDTO { UserName=dto.UserName,Password=dto.Password});
                return Ok(new Response(HttpStatusCode.OK, "Đăng ký người dùng mới thành công !", result));
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
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] RegisterUserDTO dto)
        {
            try
            {
                // Kiểm tra đăng nhập bằng stored procedure
                var parameters = new Dictionary<string, object>() {
            {"@Username", dto.UserName },
            {"@Password", dto.Password }
         };
                var loginResult = SQLHelper.ExecuteStoredProcedure("PSP_CHECKLOGIN", parameters);

                if (loginResult.Count == 0 || (bool)loginResult[0].FirstOrDefault().Value == false)
                {
                    return BadRequest(new { message = "Tài khoản hoặc mật khẩu sai!" });
                }

                // Lấy thông tin người dùng từ cơ sở dữ liệu
                var user = _context.Users
                    .Where(usn => usn.Username == dto.UserName)
                    .Select(u => new
                    {
                        u.UserId,
                        u.Username,
                        u.Email,
                        u.PhoneNumber,
                        u.Gender,
                        u.Avatar,
                        u.Role,
                        u.RoleId
                    })
                    .FirstOrDefault();

                if (user == null)
                {
                    return BadRequest(new { message = "Không tìm thấy thông tin người dùng!" });
                }

                // Tạo JWT payload
                JwtPayload payload = new JwtPayload()
                {
                    Id = user.UserId,
                    RoleId = user.RoleId,
                    UserName = user.Username
                };

                // Tạo access token và refresh token
                var accessToken = await _jwtService.GenerateJwtTokenAsync(payload);
                var refreshToken = await _jwtService.RefreshTokenAsync(payload);

                if (accessToken == null || refreshToken == null)
                {
                    return BadRequest(new { message = "Lỗi trong quá trình tạo token!" });
                }

                // Cập nhật refresh token của người dùng
                var isUpdated = await _userService.CreateOrUpdateUserWithHelper(new CreateOrUpdateUserDTO { RefreshToken=refreshToken }, user.UserId);

                if (isUpdated!=null)
                {
                    return Ok(new { user, accessToken, refreshToken });
                }

                return BadRequest(new { message = "Không thể cập nhật refresh token!", isUpdated });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
      
        [Authorize]
        [HttpPut("grant-access-tech")]

        public async Task<IActionResult> GrantAccessRole([FromQuery] string roleId, [FromQuery] string userId)
        {
            try
            {
                var token = await _authService.GetToken(Request);

                if (!await _authService.CheckRole(token))
                {
                    return Forbid();
                }

                var userUpdate = await _userService.CreateOrUpdateUserWithHelper(new CreateOrUpdateUserDTO { RoleId = roleId },userId);

                return userUpdate != null
                    ? Ok(new { message = "Cập nhật quyền cho người dùng thành công!", data = userUpdate })
                    : BadRequest("Cập nhật quyền cho người dùng không thành công!");
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPost("refreshToken")]
        public async Task<ActionResult<Response>> RefreshToken([FromBody] RefreshTokenDTO dto)
        {
            try
            {

                string accessToken = "";
                string refreshToken = "";
                var token=await _authService.GetToken(Request);
               
                if (token.IsNullOrEmpty())
                {
                    return Unauthorized();
                }
                var check = await _jwtService.ValidateRefreshTokenAsync(dto.RefreshToken);
                if (!check) return BadRequest(new Response(HttpStatusCode.BadRequest, "RefreshToken is valid"));
                

                var decodeToken = await _jwtService.DecodeJwtToken(token);

                if (decodeToken != null)
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
                return Ok(
                    new
                    {
                        HttpContext.User.Identities
                    });
                //var decodeToken = await _jwtService.DecodeJwtToken(token.AccessToken);

                //return Ok(new
                //{
                //    decodeToken
                //});
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
