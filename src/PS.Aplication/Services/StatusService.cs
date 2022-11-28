using AutoMapper;
using PS.Aplication.Dtos;
using PS.Aplication.Interfaces;
using PS.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Aplication.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusPersist _statusRepository;
        private readonly IMapper _mapper;

        public StatusService(IMapper mapper, IStatusPersist statusPersist)
        {
            _statusRepository = statusPersist;
            _mapper = mapper;
        }

        public async Task<List<StatusDto>> GetStatusesByProjectId(int projectId)
        {
            try
            {
                var statuses = await _statusRepository.GetByProjectIdAsync(projectId);

                if (statuses != null && statuses.Count > 0)
                {
                    return _mapper.Map<List<StatusDto>>(statuses);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }
    }
}
