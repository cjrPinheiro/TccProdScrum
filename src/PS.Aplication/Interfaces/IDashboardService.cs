using PS.Aplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Aplication.Interfaces
{
    public interface IDashboardService
    {
        Task<GenericChart> GenProjectsOverviewChart(int userId, int jiraDomainid, DateTime initialDate, DateTime endDate);
        Task<GenericChart> GenMemberAvgHistoryChart(int memberId, int projectId, DateTime initialDate, DateTime endDate);
    }
}
