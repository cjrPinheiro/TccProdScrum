using PS.Domain.Entities;
using PS.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Persistence.Interfaces
{
    public interface ISprintPersist : IBasePersist<Sprint>
    {
        Task<Sprint> GetByIdAsync(int sprintId);
        Task<List<Sprint>> GetByProjectIdAsync(int projectId);
        Task<Sprint> GetBySprintCodeAsync(int projectId, int id);
    }
}
