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
    public class DeviceTypesService : IDeviceTypesService
    {
        private readonly IBaseRepository<DeviceType> _deviceTypeRepository;
        private readonly AppDbContext _dbContext;
        public DeviceTypesService(IBaseRepository<DeviceType> deviceTypeRepository, AppDbContext appDbContext)
        {
            _deviceTypeRepository = deviceTypeRepository;
            _dbContext = appDbContext;
        }

        public async Task<DeviceType> CreateOrUpdateDeviceTypeAsync(CreateOrUpdateDeviceTypeDTO deviceType, int? id)
        {
            // Define an output parameter for TypeId
            var typeIdParam = new SqlParameter("@TypeId", SqlDbType.Int);

            if (id.HasValue)
            {
                typeIdParam.Value = id; // Use id for update operation
            }
            else
            {
                typeIdParam.Direction = ParameterDirection.Output;
            }

            // Define other input parameters for the stored procedure
            SqlParameter[] parameters = new SqlParameter[]
            {
        typeIdParam,
        new SqlParameter("@TypeName", SqlDbType.NVarChar, 255) { Value = deviceType.TypeName },
        new SqlParameter("@IsDelete", SqlDbType.Bit) { Value = deviceType.IsDelete }
            };

            // Execute the stored procedure with input and output parameters
            _dbContext.Database.ExecuteSqlRaw("EXEC PSP_DeviceType_InsertAndUpdate @TypeId OUTPUT, @TypeName, @IsDelete", parameters);

            // Get the TypeId from the output parameter
            var typeId = (int)typeIdParam.Value;

            // Retrieve the updated/inserted device type based on the returned TypeId
            var updatedDeviceType = _dbContext.DeviceTypes.Single(dt => dt.TypeId == typeId);
            return updatedDeviceType;
        }


        public Task<DeviceType> GetCustomerByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<DeviceType> GetDeviceTypeByIdAsync(int id)
        {
            return await _deviceTypeRepository.GetById(id);
        }

        public PagedList<DeviceType> GetDeviceTypes(PaginationFilterDto dto)
        {
            try
            {
                FormattableString sqlQuery = $"EXEC PSP_DeviceType_Select @TypeId={0}";
                var result = SharedClass.GetDataEntitiesWithPaging<DeviceType>(
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
