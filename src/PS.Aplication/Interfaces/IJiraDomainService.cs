using PS.Aplication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Aplication.Interfaces
{
    public interface IJiraDomainService
    {
        Task<JiraDomainDto> AddDomainAsync(int userId, JiraDomainEditedDto domain);
        Task<JiraDomainDto> UpdateDomain(int userId, int domainId, JiraDomainEditedDto editedDomain);
        Task<List<JiraDomainDto>> GetDomainsByUserId(int userId);
        Task<bool> DomainExists(int userId, int domainId);
        Task<bool> DomainExists(int userId, string baseUrl);
        Task<bool> DeleteDomain(int userId, int id);
    }
}
