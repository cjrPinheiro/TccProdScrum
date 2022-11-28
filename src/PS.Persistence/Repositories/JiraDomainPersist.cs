using Microsoft.EntityFrameworkCore;
using PS.Domain.Entities;
using PS.Persistence.Interfaces;
using PS.Persistence.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PS.Persistence.Repositories
{
    public class JiraDomainPersist : BasePersist<JiraDomain>, IJiraDomainPersist
    {
        public JiraDomainPersist(PSContext context) : base(context) { }

        public async Task<JiraDomain> GetByIdAsync(int userId, int id)
        {
            IQueryable<JiraDomain> query = _context.JiraDomains
                                           .Include(e => e.Projects)
                                           .ThenInclude(e => e.TeamMembers);

            query = query.Where(q => q.Id == id && q.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<JiraDomain> GetByUrlAsync(int userId, string baseUrl)
        {
            IQueryable<JiraDomain> query = _context.JiraDomains
                                           .Include(e => e.Projects)
                                           .ThenInclude(e => e.TeamMembers);

            query = query.Where(q => q.BaseUrl == baseUrl && q.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<JiraDomain>> GetByUserIdAsync(int userId)
        {
            IQueryable<JiraDomain> query = _context.JiraDomains
                                           .Include(e => e.Projects)
                                           .ThenInclude(e => e.TeamMembers);

            query = query.Where(q => q.UserId == userId);

            return await query.ToListAsync();
        }
    }
}
