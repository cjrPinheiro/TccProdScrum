using AutoMapper;
using PS.Aplication.Dtos;
using PS.Aplication.Interfaces;
using PS.Domain.Entities;
using PS.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Aplication.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectPersist _projectRepository;

        private readonly IMapper _mapper;

        public ProjectService(IMapper mapper, IProjectPersist projectPersist)
        {
            _projectRepository = projectPersist;
            _mapper = mapper;
        }
        public Task<JiraDomainEditedDto> AddProjectAsync(int jiraDomainId, ProjectDto newProject)
        {
            throw new NotImplementedException();
        }
        public async Task<ProjectDto> UpdateProject(int projectId, ProjectDto editedProject)
        {
            try
            {
                var project = await _projectRepository.GetByIdAsync(projectId);
                if (project == null) return null;

                project.DevelopingStatusId = editedProject.DevelopingStatusId;
                project.CompletedStatusId = editedProject.CompletedStatusId;
               
                _projectRepository.Update(project);

                if (await _projectRepository.SaveChangesAsync())
                {
                    var upUser = await _projectRepository.GetByIdAsync(project.Id);
                    var mapRes = _mapper.Map<ProjectDto>(upUser);
                    return mapRes;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> ProjectExists(int userId, int projectId)
        {
            try
            {
                var project = await _projectRepository.GetByIdAsync(projectId);
                if (project != null && project.JiraDomain.UserId == userId)
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProjectDto>> GetProjectsByJiraDomainId(int userId, int jiraDomainId)
        {       
            try
            {
                List<Project> projects = await _projectRepository.GetByUserIdAsync(userId, jiraDomainId);
                if (projects != null)
                {
                    var res = _mapper.Map<List<ProjectDto>>(projects);
                    foreach (var item in res)
                    {
                        var devStatus = item.Statuses.Where(q => q.Id == item.DevelopingStatusId).FirstOrDefault();
                        var completeStatus = item.Statuses.Where(q => q.Id == item.CompletedStatusId).FirstOrDefault();
                        item.DevelopingStatus = devStatus != null ? devStatus.Description : "";
                        item.CompletedStatus = completeStatus != null ? completeStatus.Description : "";
                    }
                    return res;
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

      
    }
}
