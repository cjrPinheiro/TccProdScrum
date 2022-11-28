using PS.Aplication.Dtos;
using PS.Atlassian.Connector.Models.Requests.TeamMember;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace PS.Aplication.Interfaces
{
    public interface IJiraService
    {
        Task<JiraDomainEditedDto> CreateJiraDomainAsync(int userId, JiraDomainEditedDto newDomain);
        Task<ProjectDto> AddMemberAsync(int projectId, TeamMemberDto newMember);
        Task<bool> DomainExists(int userId, string baseUrl);
        Task<bool> DomainExists(int userId, int jiraDomainid);
        Task<int> ImportProjects(int userId, int jiraDomainid);
        Task<int> ImportStatuses(int userId, int jiraDomainid);
        Task<int> ImportTeamMembers(int userId, int projectId);
        Task<int> ImportSprints(int userId, int projectId);
        Task<List<TeamMemberPerformance>> GetIssuesByDoneStatuses(int userId, int projectId, DateTime initialDate, DateTime endDate);
        Task<List<TeamMemberPerformance>> GetIssuesBySprintId(int userId, int sprintId);
        Task<List<SprintDto>> GetSprintsByProjectId(int userId, int jiraDomainId);
    }
}
