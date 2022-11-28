using PS.Aplication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Aplication.Interfaces
{
    public interface IProjectService
    {
        Task<JiraDomainEditedDto> AddProjectAsync(int jiraDomainId, ProjectDto newProject);
        Task<ProjectDto> UpdateProject(int projectId, ProjectDto editedProject);
        Task<List<ProjectDto>> GetProjectsByJiraDomainId(int userId, int jiraDomainId);
        Task<bool> ProjectExists(int userId, int projectId);
    }
}
