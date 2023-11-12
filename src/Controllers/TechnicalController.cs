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
    [Route("api/[controller]",Name ="Technical")]
    [ApiController]
    public class TechnicalController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITechnicalService _technicalService;

        public TechnicalController(IAuthService authService, ITechnicalService technicalService)
        {
            _authService = authService;
            _technicalService = technicalService;
        }
        [HttpPost("create")]
        public async Task<ActionResult<Response>> CreateCustomer(CreateOrUpdateTechnicalDTO dto)
        {
            try
            {
                var customer = await _technicalService.CreateOrUpdateTechnicalAsync(dto);
                return Ok(new Response(HttpStatusCode.OK, "Tạo thông tin kĩ thuật mới thành công !", customer));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpGet("gets")]

        public async Task<ActionResult<Response>> GetTechnicals([FromQuery] PaginationFilterDto filter)
        {
            var technicals = _technicalService.GetTechnicals(filter);

            var metadata = technicals.GetMetadata();
            return Ok(new Response(HttpStatusCode.OK, "Lấy danh sách kĩ thuật thành công", new
            {
                technicals,
                metadata
            }));

        }
        [HttpGet("get-technical-by-id")]

        public async Task<ActionResult<Response>> GetTechnicalById([FromQuery, Required] int id)
        {
            try
            {
                var technical = await _technicalService.GetTechnicalByIdAsync(id);
                if (technical == null)
                {
                    return NotFound(new Response(HttpStatusCode.NotFound, "Technical not found"));
                }
                return Ok(new Response(HttpStatusCode.OK, "Lấy kĩ thuật thành công !", technical));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(HttpStatusCode.BadRequest, ex.Message));
            }
        }
        [HttpPut("update")]
        public async Task<ActionResult<Response>> UpdateTechnical([FromQuery, Required] int id, [FromBody] CreateOrUpdateTechnicalDTO dto)
        {
            try
            {
                var technical = await _technicalService.CreateOrUpdateTechnicalAsync(dto, id);
                return Ok(new Response(HttpStatusCode.OK, "Cập nhật kĩ thuật thành công !", technical));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
