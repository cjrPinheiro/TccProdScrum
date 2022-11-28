using System.Collections.Generic;
using System.Threading.Tasks;
using PS.Domain.Entities;
using PS.Persistence.Interfaces.Base;
namespace PS.Persistence.Interfaces
{
    public interface IProjectPersist : IBasePersist<Project>
    {
        Task<Project> GetByIdAsync(int projectId);
        Task<List<Project>> GetByUserIdAsync(int userId, int jiraDomainId);
        Task<Project> GetByKeyAsync(int domainId, string key);
        Task<Project> GetByCodeAsync(int domainId, int code);
        Task<List<Project>> GetByDomainIdAsync(int jiraDomainid);
    }
}
