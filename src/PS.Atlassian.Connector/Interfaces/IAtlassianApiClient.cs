using PS.Atlassian.Connector.Models.Requests;
using PS.Atlassian.Connector.Models.Requests.Issues;
using PS.Atlassian.Connector.Models.Requests.Project;
using PS.Atlassian.Connector.Models.Requests.Sprint;
using PS.Atlassian.Connector.Models.Requests.Status;
using PS.Atlassian.Connector.Models.Requests.TeamMember;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PS.Atlassian.Connector.Interfaces
{
    public interface IAtlassianApiClient
    {
        Task<ProjectResponse[]> GetProjectsAsync();
        Task<TeamMemberResponse> GetTeamMembersAsync(string projectKey);
        Task<List<TeamMemberPerformance>> GetIssuesByStatusChanged(string developingStatus, string completedStatus, string projectKey, DateTime initialDate, DateTime endDate, int workHours);
        Task<List<TeamMemberPerformance>> GetTeamPerformanceBySprintCode(int sprintCode, int projectCode, DateTime initialDate, DateTime endDate, int workHours);
        Task<List<Sprint>> GetSprintsByProjectCode(int projectCode);
        Task<StatusesResponse[]> GetAllStatusesAsync();
    }
}
