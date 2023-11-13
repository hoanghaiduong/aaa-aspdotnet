using aaa_aspdotnet.src.BAL.IServices;
using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.pagination;
using aaa_aspdotnet.src.Common.shared;
using aaa_aspdotnet.src.DAL.Entities;
using aaa_aspdotnet.src.DAL.IRepositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace aaa_aspdotnet.src.BAL.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IBaseRepository<Device> _deviceRepository;
        private readonly AppDbContext _dbContext;

        public DeviceService(IBaseRepository<Device> deviceRepository, AppDbContext dbContext)
        {
            _deviceRepository = deviceRepository;
            _dbContext = dbContext;
        }

        public async Task<Device> CreateOrUpdateDeviceAsync(CreateOrUpdateDeviceDTO device, int? id = null)
        {
            var deviceIDParam = new SqlParameter("@DeviceID", SqlDbType.Int);

            if (id.HasValue)
            {
                deviceIDParam.Value = id; // Use id for update operation
            }
            else
            {
                deviceIDParam.Direction = ParameterDirection.Output;
            }

            // Define other input parameters for the stored procedure
            SqlParameter[] parameters = new SqlParameter[]
            {
                deviceIDParam,
                new SqlParameter("@TypeID", SqlDbType.Int) { Value = device.TypeID },
                new SqlParameter("@DeviceName", SqlDbType.NVarChar, 255) { Value = device.DeviceName },
                new SqlParameter("@Code", SqlDbType.VarChar, 50) { Value = device.Code },
                new SqlParameter("@Color", SqlDbType.VarChar, 50) { Value = device.Color },
                new SqlParameter("@Descriptions", SqlDbType.NVarChar, 255) { Value = device.Descriptions },
                new SqlParameter("@QRCode", SqlDbType.VarChar, 255) { Value = device.QRCode },
                new SqlParameter("@IsDelete", SqlDbType.Bit) { Value = device.IsDelete },
                new SqlParameter("@FacID", SqlDbType.Int) { Value = device.FacID }
            };

            // Execute the stored procedure with input and output parameters
            _dbContext.Database.ExecuteSqlRaw("EXEC PSP_Devices_InsertAndUpdate @DeviceID OUTPUT, @TypeID, @DeviceName, @Code, @Color, @Descriptions, @QRCode, @IsDelete, @FacID", parameters);

            // Get the DeviceID from the output parameter
            var deviceID = (int)deviceIDParam.Value;

            // Retrieve the updated/inserted device based on the returned DeviceID
            var updatedDevice = _dbContext.Devices.Single(d => d.DeviceId == deviceID);
            return  updatedDevice;

        }

        public async Task<Device> GetDeviceByIdAsync(int id)
        {
            return await _deviceRepository.GetById(id);
        }

        public Task<Device> GetDeviceByNameAsync(string name)
        {
           throw new NotImplementedException();
        }

        public PagedList<Device> GetDevices(PaginationFilterDto dto)
        {
            try
            {
                FormattableString sqlQuery = $"EXEC PSP_Devices_Select @DeviceID={0}";
                var result = SharedClass.GetDataEntitiesWithPaging<Device>(
                    _dbContext,
                    sqlQuery,
                    dto);

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
