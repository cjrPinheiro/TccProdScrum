using Microsoft.EntityFrameworkCore;
using PS.Domain.Entities;
using PS.Persistence.Interfaces;
using PS.Persistence.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Persistence.Repositories
{
    public class SprintPersist : BasePersist<Sprint>, ISprintPersist
    {
        public SprintPersist(PSContext context) : base(context) { }

        public async Task<Sprint> GetByIdAsync(int sprintId)
        {
            IQueryable<Sprint> query = _context.Sprints
                                     .Include(e => e.TeamMembers)
                                     .Include(e => e.Project)
                                     .ThenInclude(e=> e.JiraDomain);


            query = query.Where(q => q.Id == sprintId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Sprint>> GetByProjectIdAsync(int projectId)
        {
            IQueryable<Sprint> query = _context.Sprints;

            query = query.Where(q => q.ProjectId == projectId);

            return await query.ToListAsync();
        }

        public async Task<Sprint> GetBySprintCodeAsync(int projectId, int sprintCode)
        {
            IQueryable<Sprint> query = _context.Sprints;

            query = query.Where(q => q.SprintCode == sprintCode && q.ProjectId == projectId);

            return await query.FirstOrDefaultAsync();
        }
    }
}
