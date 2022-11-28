using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS.Domain.Entities;
using PS.Persistence.Interfaces.Base;
namespace PS.Persistence.Interfaces
{
    public interface ITeamMemberPersist : IBasePersist<TeamMember>
    {
        Task<TeamMember> GetByEmailAsync(string emailAddress);
        Task<TeamMember> GetByAccounIdAsync(string accountId);
        Task<TeamMember> GetByIdAsync(int id);
        Task<TeamMember> GetByIdAndProjectIdAsync(int id, int projectId, DateTime initialDate, DateTime endDate);

    }
}
