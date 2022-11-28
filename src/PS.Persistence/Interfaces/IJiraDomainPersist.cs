using PS.Domain.Entities;
using PS.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Persistence.Interfaces
{
    public interface IJiraDomainPersist : IBasePersist<JiraDomain>
    {
        Task<JiraDomain> GetByIdAsync(int userId, int id);
        Task<JiraDomain> GetByUrlAsync(int userId, string baseUrl);
        Task<List<JiraDomain>> GetByUserIdAsync(int userId);
    }
}
