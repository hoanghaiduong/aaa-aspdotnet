using aaa_aspdotnet.src.BAL.IServices;
using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace aaa_aspdotnet.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IDeviceService _deviceService;

        public DeviceController(IAuthService authService, IDeviceService deviceService)
        {
            _authService = authService;
            _deviceService = deviceService;
        }
        [HttpPost("create")]
        public async Task<ActionResult<Response>> CreateDevice(CreateOrUpdateDeviceDTO dto)
        {
            try
            {
                var device = await _deviceService.CreateOrUpdateDeviceAsync(dto);
                return Ok(new Response(HttpStatusCode.OK, "Tạo thiết bị mới thành công !", device));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpGet("gets")]

        public async Task<ActionResult<Response>> GetDevices([FromQuery] PaginationFilterDto filter)
        {
            var devices = _deviceService.GetDevices(filter);

            var metadata = devices.GetMetadata();
            return Ok(new Response(HttpStatusCode.OK, "Lấy danh sách thiết bị thành công", new
            {
                devices,
                metadata
            }));

        }
        [HttpGet("get-device-by-id")]

        public async Task<ActionResult<Response>> GetDeviceById([FromQuery, Required] int id)
        {
            try
            {
                var devices = await _deviceService.GetDeviceByIdAsync(id);
                if (devices == null)
                {
                    return NotFound(new Response(HttpStatusCode.NotFound, "Device not found"));
                }
                return Ok(new Response(HttpStatusCode.OK, "Lấy thiết bị thành công !", devices));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, ex.Message));
            }
        }
        [HttpPut("update")]
        public async Task<ActionResult<Response>> UpdateDevice([FromQuery, Required] int id, [FromBody] CreateOrUpdateDeviceDTO dto)
        {
            try
            {
                var device = await _deviceService.CreateOrUpdateDeviceAsync(dto, id);
                return Ok(new Response(HttpStatusCode.OK, "Cập nhật thiết bị thành công !", device));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
