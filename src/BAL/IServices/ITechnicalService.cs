using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.pagination;
using aaa_aspdotnet.src.DAL.Entities;
using aaa_aspdotnet.src.DAL.IRepositories;

namespace aaa_aspdotnet.src.BAL.IServices
{
    
    public interface ITechnicalService 
    {
        Task<Technical> CreateOrUpdateTechnicalAsync(CreateOrUpdateTechnicalDTO technical, int? id = null);
       
        Task<Technical> GetTechnicalByIdAsync(int id);
        Task<Technical> GetTechnicalByNameAsync(string name);
        PagedList<Technical> GetTechnicals(PaginationFilterDto dto);
    }
}
