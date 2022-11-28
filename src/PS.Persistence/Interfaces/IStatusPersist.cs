using PS.Domain.Entities;
using PS.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Persistence.Interfaces
{
    public interface IStatusPersist : IBasePersist<Status>
    {
        Task<Status> GetByIdAsync(int statusId);
        Task<List<Status>> GetByProjectIdAsync(int projectId);
        Task<Status> GetByStatusCodeAsync(int projectId, int jitaCode);

    }
}
