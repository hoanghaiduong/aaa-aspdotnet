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
   
    public class TechnicalService : ITechnicalService
    {
        private readonly IBaseRepository<Technical> _technicalRepository;
        private readonly AppDbContext _dbContext;

        public TechnicalService(IBaseRepository<Technical> technicalRepository, AppDbContext appDbContext)
        {
            _technicalRepository = technicalRepository;
            _dbContext = appDbContext;
        }

        public async Task<Technical> CreateOrUpdateTechnicalAsync(CreateOrUpdateTechnicalDTO technical, int? id = null)
        {
            var technicalIDParam = new SqlParameter("@TechnicalID", SqlDbType.Int);

            if (id.HasValue)
            {
                technicalIDParam.Value = id; // Use id for update operation
            }
            else
            {
                technicalIDParam.Direction = ParameterDirection.Output;
            }

            // Define input parameters for the stored procedure
            SqlParameter[] parameters = new SqlParameter[]
            {
                technicalIDParam,
                new SqlParameter("@TechnicalName", SqlDbType.NVarChar, 255) { Value = technical.TechnicalName },
                new SqlParameter("@Address", SqlDbType.NVarChar, 255) { Value = technical.Address },
                new SqlParameter("@Phone", SqlDbType.VarChar, 20) { Value = technical.Phone },
                new SqlParameter("@Phone2", SqlDbType.VarChar, 20) { Value = technical.Phone2 },
                new SqlParameter("@Zalo", SqlDbType.NVarChar, 255) { Value = technical.Zalo },
                new SqlParameter("@Email", SqlDbType.VarChar, 255) { Value = technical.Email },
                new SqlParameter("@IsDelete", SqlDbType.Bit) { Value = technical.IsDelete }
            };

            // Execute the stored procedure with input and output parameters
            _dbContext.Database.ExecuteSqlRaw("EXEC PSP_Technical_InsertAndUpdate @TechnicalID OUTPUT, @TechnicalName, @Address, @Phone, @Phone2, @Zalo, @Email, @IsDelete", parameters);

            // Get the TechnicalID from the output parameter
            var technicalID = (int)technicalIDParam.Value;

            // Retrieve the updated/inserted technical based on the returned TechnicalID
            var updatedTechnical = _dbContext.Technicals.Single(t => t.TechnicalId == technicalID);
            return updatedTechnical;
        }

        public async Task<Technical> GetTechnicalByIdAsync(int id)
        {
            return await _technicalRepository.GetById(id);
        }

        public Task<Technical> GetTechnicalByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public PagedList<Technical> GetTechnicals(PaginationFilterDto dto)
        {
            try
            {
                FormattableString sqlQuery = $"EXEC PSP_Technical_Select @TechnicalID={0}";
                var result = SharedClass.GetDataEntitiesWithPaging<Technical>(
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
