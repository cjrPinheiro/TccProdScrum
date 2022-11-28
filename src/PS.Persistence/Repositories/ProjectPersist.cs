using Microsoft.EntityFrameworkCore;
using PS.Domain.Entities;
using PS.Persistence.Interfaces;
using PS.Persistence.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PS.Persistence.Repositories
{
    public class ProjectPersist : BasePersist<Project>, IProjectPersist
    {
        public ProjectPersist(PSContext context) : base(context) { }

        public async Task<Project> GetByIdAsync(int projectId)
        {
            IQueryable<Project> query = _context.Projects
                                          .Include(e => e.TeamMembers)
                                          .Include(e=> e.Statuses)
                                          .Include(e => e.JiraDomain);
                                          

            query = query.Where(q => q.Id == projectId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Project> GetByKeyAsync(int domainId, string key)
        {
            IQueryable<Project> query = _context.Projects;

            query = query.Where(q => q.Key == key && q.JiraDomainId == domainId);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<Project> GetByCodeAsync(int domainId, int code)
        {
            IQueryable<Project> query = _context.Projects;

            query = query.Where(q => q.JiraBoardCode == code && q.JiraDomainId == domainId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Project>> GetByUserIdAsync(int userId, int jiraDomainId)
        {
            IQueryable<Project> query = _context.Projects.Include(q=>q.Statuses);
                                        
            query = query.Where(q => q.JiraDomainId == jiraDomainId && q.JiraDomain.UserId == userId);

            return await query.ToListAsync();
        }

        public async Task<List<Project>> GetByDomainIdAsync(int jiraDomainid)
        {
            IQueryable<Project> query = _context.Projects;

            query = query.Where(q => q.JiraDomainId == jiraDomainid);

            return await query.ToListAsync();
        }
    }
}
