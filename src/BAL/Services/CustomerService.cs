using aaa_aspdotnet.src.BAL.IServices;
using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.Enums;
using aaa_aspdotnet.src.Common.Helpers;
using aaa_aspdotnet.src.Common.pagination;
using aaa_aspdotnet.src.Common.shared;
using aaa_aspdotnet.src.DAL.Entities;
using aaa_aspdotnet.src.DAL.IRepositories;
using aaa_aspdotnet.src.DAL.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace aaa_aspdotnet.src.BAL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IBaseRepository<Customer> _repository;
        private readonly AppDbContext _dbContext;
        public CustomerService(IBaseRepository<Customer> repository, AppDbContext dbContext)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<Customer> CreateOrUpdateCustomerAsync(CreateOrUpdateCustomerDTO customer, int? id)
        {
            // Define an output parameter for CustomerID
            var customerIDParam = new SqlParameter("@CustomerID", SqlDbType.Int);
           

            if (id.HasValue)
            {
                customerIDParam.Value = id; // Use id for update operation
            }
            else
            {
                customerIDParam.Direction = ParameterDirection.Output;
            }
            // Define other input parameters for the stored procedure
            SqlParameter[] parameters = new SqlParameter[]
            {
                customerIDParam,
                new SqlParameter("@CustomerName", SqlDbType.NVarChar, 50) { Value = customer.CustomerName },
                new SqlParameter("@Address", SqlDbType.NVarChar, 255) { Value = customer.Address },
                new SqlParameter("@Phone", SqlDbType.VarChar, 20) { Value = customer.Phone },
                new SqlParameter("@Phone2", SqlDbType.VarChar, 20) { Value = customer.Phone2 },
                new SqlParameter("@Email", SqlDbType.VarChar, 20) { Value = customer.Email }
            };

            // Execute the stored procedure with input and output parameters
            _dbContext.Database.ExecuteSqlRaw("EXEC PSP_Customer_InsertAndUpdate @CustomerID OUTPUT, @CustomerName, @Address, @Phone, @Phone2, @Email", parameters);

            // Get the CustomerID from the output parameter
            var customerID = (int)customerIDParam.Value;

            // Retrieve the updated/inserted customer based on the returned CustomerID
            var updatedCustomer = _dbContext.Customers.Single(c => c.CustomerId == customerID);
            return updatedCustomer;
        }




        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _repository.GetById(id);
        }

        public Task<Customer> GetCustomerByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public  PagedList<Customer> GetCustomers(PaginationFilterDto dto)
        {
            try
            {
                FormattableString sqlQuery = $"EXEC PSP_Customer_Select @CustomerID={0}";
                var result = SharedClass.GetDataEntitiesWithPaging<Customer>(
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
