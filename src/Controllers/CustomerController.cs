using aaa_aspdotnet.src.BAL.IServices;
using aaa_aspdotnet.src.BAL.Services;
using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace aaa_aspdotnet.src.Controllers
{
    [Route("api/[controller]",Name ="Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICustomerService _customerService;

        public CustomerController(IAuthService authService, ICustomerService customerService)
        {
            _authService = authService;
            _customerService = customerService;
        }
        [HttpPost("create")]
        public async Task<ActionResult<Response>> CreateCustomer(CreateOrUpdateCustomerDTO dto)
        {
            try
            {
                var customer = await _customerService.CreateOrUpdateCustomerAsync(dto);
                return Ok(new Response(HttpStatusCode.OK,"Tạo người dùng mới thành công !", customer));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
        }
        [HttpGet("gets")]

        public async Task<ActionResult<Response>> GetUsers([FromQuery] PaginationFilterDto filter)
        {
            var customers = _customerService.GetCustomers(filter);
           
            var metadata = customers.GetMetadata();
            return Ok(new Response(HttpStatusCode.OK, "Lấy danh sách khách hàng thành công", new
            {
                customers,
                metadata
            }));
           
        }
        [HttpGet("get-customer-by-id")]

        public async Task<ActionResult<Response>> GetUserById([FromQuery,Required] int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return NotFound(new Response(HttpStatusCode.NotFound, "Customer not found"));
                }
                return Ok(new Response(HttpStatusCode.OK,"Lấy người dùng thành công !", customer));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, ex.Message));
            }
        }
        [HttpPut("update")]
        public async Task<ActionResult<Response>> UpdateCustomer([FromQuery,Required] int id,[FromBody] CreateOrUpdateCustomerDTO dto)
        {
            try
            {
                var customer = await _customerService.CreateOrUpdateCustomerAsync(dto,id);
                return Ok(new Response(HttpStatusCode.OK,"Cập nhật người dùng thành công !", customer));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
