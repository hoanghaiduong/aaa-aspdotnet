using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.pagination;
using aaa_aspdotnet.src.DAL.Entities;

namespace aaa_aspdotnet.src.BAL.IServices
{
    public interface IUserService
    {
      //  Task<Response> CreateUser(CreateUserDTO dto);
        Task<User> GetUserById(string id);
        Task<User> CreateOrUpdateUserWithHelper(CreateOrUpdateUserDTO dto, string? userId=null);
        PagedList<User> GetUsers(PaginationFilterDto dto);
    }
}
