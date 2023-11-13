using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.pagination;
using aaa_aspdotnet.src.DAL.Entities;

namespace aaa_aspdotnet.src.BAL.IServices
{
    public interface IDeviceService
    {
        Task<Device> CreateOrUpdateDeviceAsync(CreateOrUpdateDeviceDTO device, int? id = null);
        
        Task<Device> GetDeviceByIdAsync(int id);
        Task<Device> GetDeviceByNameAsync(string name);
        PagedList<Device> GetDevices(PaginationFilterDto dto);
    }
}
