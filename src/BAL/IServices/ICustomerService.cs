using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.pagination;
using aaa_aspdotnet.src.DAL.Entities;

namespace aaa_aspdotnet.src.BAL.IServices
{
    public interface ICustomerService
    {
        Task<Customer> CreateOrUpdateCustomerAsync(CreateOrUpdateCustomerDTO customer,int? id=null);
        //Task<Customer> UpdateCustomerAsync(CreateOrUpdateCustomerDTO customer, int id);
        //Task<Customer> DeleteCustomerAsync(CreateOrUpdateCustomerDTO customer, int id);
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<Customer> GetCustomerByNameAsync(string name);
        PagedList<Customer> GetCustomers(PaginationFilterDto dto);
    }
}
