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
    public class StatusPersist : BasePersist<Status>, IStatusPersist
    {
        public StatusPersist(PSContext context) : base(context) { }

        public async Task<Status> GetByIdAsync(int statusId)
        {
            IQueryable<Status> query = _context.Statuses;
                                     //.Include(e => e.Project)
                                    // .ThenInclude(e => e.JiraDomain);

            query = query.Where(q => q.Id == statusId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Status>> GetByProjectIdAsync(int projectId)
        {
            IQueryable<Status> query = _context.Statuses;

            query = query.Where(q => q.ProjectId == projectId);

            return await query.ToListAsync();
        }

        public async Task<Status> GetByStatusCodeAsync(int projectId, int jiraCode)
        {
            IQueryable<Status> query = _context.Statuses;

            query = query.Where(q => q.JiraStatusCode == jiraCode && q.ProjectId == projectId);

            return await query.FirstOrDefaultAsync();
        }
    }
}
