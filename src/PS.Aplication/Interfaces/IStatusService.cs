using PS.Aplication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Aplication.Interfaces
{
    public interface IStatusService
    {
        Task<List<StatusDto>> GetStatusesByProjectId(int projectId);
    }
}
