using aaa_aspdotnet.src.BAL.IServices;
using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.pagination;
using aaa_aspdotnet.src.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace aaa_aspdotnet.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceTypeController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IDeviceTypesService _deviceTypeService;

        public DeviceTypeController(IAuthService authService, IDeviceTypesService deviceTypeService)
        {
            _authService = authService;
            _deviceTypeService = deviceTypeService;
        }
        [HttpPost("create")]
        public async Task<ActionResult<Response>> CreateDeviceType(CreateOrUpdateDeviceTypeDTO dto)
        {
            try
            {
                var customer = await _deviceTypeService.CreateOrUpdateDeviceTypeAsync(dto);
                return Ok(new Response(HttpStatusCode.OK, "Tạo người dùng mới thành công !", customer));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpGet("gets")]

        public async Task<ActionResult<Response>> GetDeviceTypes([FromQuery] PaginationFilterDto filter)
        {
            var deviceTypes = _deviceTypeService.GetDeviceTypes(filter);

            var metadata = deviceTypes.GetMetadata();
            return Ok(new Response(HttpStatusCode.OK, "Lấy danh sách loại thiết bị thành công", new
            {
                deviceTypes,
                metadata
            }));

        }
        [HttpGet("get-device-type-by-id")]

        public async Task<ActionResult<Response>> GetDeviceTypeById([FromQuery, Required] int id)
        {
            try
            {
                var deviceType = await _deviceTypeService.GetDeviceTypeByIdAsync(id);
                if (deviceType == null)
                {
                    return NotFound(new Response(HttpStatusCode.NotFound, "Device Type not found"));
                }
                return Ok(new Response(HttpStatusCode.OK, "Lấy loại thiết bị thành công !", deviceType));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, ex.Message));
            }
        }
        [HttpPut("update")]
        public async Task<ActionResult<Response>> UpdateDeviceType([FromQuery, Required] int id, [FromBody] CreateOrUpdateDeviceTypeDTO dto)
        {
            try
            {
                var deviceType = await _deviceTypeService.CreateOrUpdateDeviceTypeAsync(dto, id);
                return Ok(new Response(HttpStatusCode.OK, "Cập nhật loại thiết bị thành công !", deviceType));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
