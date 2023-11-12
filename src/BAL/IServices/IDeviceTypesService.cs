using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.pagination;
using aaa_aspdotnet.src.DAL.Entities;

namespace aaa_aspdotnet.src.BAL.IServices
{
    public interface IDeviceTypesService
    {
        Task<DeviceType> CreateOrUpdateDeviceTypeAsync(CreateOrUpdateDeviceTypeDTO deviceType, int? id = null);
    
        Task<DeviceType> GetDeviceTypeByIdAsync(int id);
        Task<DeviceType> GetCustomerByNameAsync(string name);
        PagedList<DeviceType> GetDeviceTypes(PaginationFilterDto dto);
    }
}
