using Microsoft.EntityFrameworkCore;
using PS.Domain.Entities;
using PS.Persistence.Interfaces;
using PS.Persistence.Repositories.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PS.Persistence.Repositories
{
    public class TeamMemberPersist : BasePersist<TeamMember>, ITeamMemberPersist
    {
        public TeamMemberPersist(PSContext context) : base(context) { }

        public async Task<TeamMember> GetByAccounIdAsync(string accountId)
        {
            IQueryable<TeamMember> query = _context.TeamMembers;

            query = query.Where(q => q.JiraAccountId == accountId);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<TeamMember> GetByIdAsync(int id)
        {
            IQueryable<TeamMember> query = _context.TeamMembers;

            query = query.Where(q => q.Id == id).Include(q => q.Sprints).ThenInclude(q => q.Sprint)
                                                .Include(q=> q.Projects);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<TeamMember> GetByIdAndProjectIdAsync(int id, int projectId, DateTime initialDate, DateTime endDate)
        {
            IQueryable<TeamMember> query = _context.TeamMembers;

            query = query.Where(q => q.Id == id && q.Sprints.Any(q => q.Sprint.ProjectId == projectId &&
                                                                 q.Sprint.StartDate >= initialDate && q.Sprint.EndDate <= endDate))
                                                .Include(q => q.Sprints).ThenInclude(q => q.Sprint)
                                                .Include(q => q.Projects);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<TeamMember> GetByEmailAsync(string emailAddress)
        {
            IQueryable<TeamMember> query = _context.TeamMembers;

            query = query.Where(q => q.Email == emailAddress);

            return await query.FirstOrDefaultAsync();
        }
    }
}
