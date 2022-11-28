using AutoMapper;
using PS.Aplication.Dtos;
using PS.Aplication.Interfaces;
using PS.Atlassian.Connector;
using PS.Atlassian.Connector.Models.Requests.TeamMember;
using PS.Domain.Entities;
using PS.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PS.Aplication.Services
{
    public class JiraService : IJiraService
    {
        private readonly IJiraDomainPersist _jiraDomainRepository;
        private readonly IProjectPersist _projectRepository;
        private readonly ITeamMemberPersist _teamMemberRepository;
        private readonly ISprintPersist _sprintRepository;
        private readonly IStatusPersist _statusRepository;

        private readonly IMapper _mapper;

        public JiraService(IMapper mapper, IJiraDomainPersist jiraDomainPersist, IProjectPersist projectPersist,
            ITeamMemberPersist teamMemberPersist, ISprintPersist sprintPersist, IStatusPersist statusPersist)
        {
            _jiraDomainRepository = jiraDomainPersist;
            _teamMemberRepository = teamMemberPersist;
            _projectRepository = projectPersist;
            _sprintRepository = sprintPersist;
            _statusRepository = statusPersist;
            _mapper = mapper;
        }

        public Task<ProjectDto> AddMemberAsync(int projectId, TeamMemberDto newMember)
        {
            throw new NotImplementedException();
        }


        public async Task<JiraDomainEditedDto> CreateJiraDomainAsync(int userId, JiraDomainEditedDto newDomain)
        {
            try
            {
                JiraDomain jiraDomain = _mapper.Map<JiraDomain>(newDomain);
                jiraDomain.UserId = userId;
                await _jiraDomainRepository.AddAsync(jiraDomain);

                if (await _jiraDomainRepository.SaveChangesAsync())
                {
                    var result = await _jiraDomainRepository.GetByIdAsync(userId, jiraDomain.Id);
                    return _mapper.Map<JiraDomainEditedDto>(result);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DomainExists(int userId, string baseUrl)
        {
            try
            {
                if (await _jiraDomainRepository.GetByUrlAsync(userId, baseUrl) != null)
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DomainExists(int userId, int jiraDomainid)
        {
            try
            {
                if (await _jiraDomainRepository.GetByIdAsync(userId, jiraDomainid) != null)
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> ImportProjects(int userId, int jiraDomainid)
        {
            try
            {
                var jiraDomain = await _jiraDomainRepository.GetByIdAsync(userId, jiraDomainid);
                int totalImports = 0;
                if (jiraDomain != null)
                {
                    var apiClient = new AtlassianApiClient(jiraDomain.Email, jiraDomain.ApiKey, jiraDomain.BaseUrl);
                    var projectsResponse = await apiClient.GetProjectsAsync();

                    foreach (var project in projectsResponse)
                    {
                        if (await _projectRepository.GetByKeyAsync(jiraDomain.Id, project.key) == null)
                        {
                            await _projectRepository.AddAsync(new Project()
                            {
                                JiraDomainId = jiraDomain.Id,
                                Key = project.key,
                                Name = project.name,
                                JiraBoardCode = int.Parse(project.id)

                            });
                            totalImports += 1;
                        }
                    }

                    if (await _projectRepository.SaveChangesAsync())
                    {
                        return totalImports;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return 0;
        }
        public async Task<int> ImportStatuses(int userId, int jiraDomainid)
        {
            try
            {
                var jiraDomain = await _jiraDomainRepository.GetByIdAsync(userId, jiraDomainid);
                int totalImports = 0;
                if (jiraDomain != null)
                {
                    var apiClient = new AtlassianApiClient(jiraDomain.Email, jiraDomain.ApiKey, jiraDomain.BaseUrl);
                    var statusesResponse = await apiClient.GetAllStatusesAsync();

                    foreach (var status in statusesResponse)
                    {
                        if (status.scope != null)
                        {
                            var project = await _projectRepository.GetByCodeAsync(jiraDomainid, int.Parse(status.scope.project.id));
                            if (project != null)
                            {
                                if (!string.IsNullOrWhiteSpace(status.id) && _statusRepository.GetByStatusCodeAsync(project.Id, int.Parse(status.id)).GetAwaiter().GetResult() == null)
                                {
                                    await _statusRepository.AddAsync(new Status()
                                    {
                                        ProjectId = project.Id,
                                        Description = status.name,
                                        JiraStatusCode = int.Parse(status.id)

                                    });
                                    totalImports += 1;
                                }
                            }
                        }
                    }

                    if (await _projectRepository.SaveChangesAsync())
                    {
                        return totalImports;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return 0;
        }
        public async Task<int> ImportTeamMembers(int userId, int projectId)
        {
            try
            {
                int totalImports = 0;
                var project = await _projectRepository.GetByIdAsync(projectId);

                if (project != null && project.JiraDomain.UserId == userId)
                {
                    var apiClient = new AtlassianApiClient(project.JiraDomain.Email, project.JiraDomain.ApiKey, project.JiraDomain.BaseUrl);
                    var membersResponse = await apiClient.GetTeamMembersAsync(project.Key);
                    if (membersResponse.total > 0)
                    {
                        foreach (var member in membersResponse.values)
                        {
                            if (await _teamMemberRepository.GetByEmailAsync(member.emailAddress) == null)
                            {
                                await _teamMemberRepository.AddAsync(new TeamMember()
                                {
                                    Email = member.emailAddress,
                                    Projects = new List<Project>() { project },
                                    JiraAccountId = member.accountId,
                                    Name = member.displayName,
                                });
                                totalImports += 1;
                            }

                        }
                    }

                    if (await _projectRepository.SaveChangesAsync())
                    {
                        return totalImports;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return 0;
        }
        public async Task<List<TeamMemberPerformance>> GetIssuesByDoneStatuses(int userId, int projectId, DateTime initialDate, DateTime endDate)
        {
            try
            {
                var project = await _projectRepository.GetByIdAsync(projectId);

                if (project != null && project.JiraDomain.UserId == userId)
                {
                    var devStatus = project.Statuses.Where(q => q.Id == project.DevelopingStatusId).FirstOrDefault();
                    var completeStatus = project.Statuses.Where(q => q.Id == project.CompletedStatusId).FirstOrDefault();
                    if (devStatus == null || completeStatus == null)
                        throw new Exception("Favor cadastrar os status para realização da busca");
                    var apiClient = new AtlassianApiClient(project.JiraDomain.Email, project.JiraDomain.ApiKey, project.JiraDomain.BaseUrl);
                    var membersResponse = await apiClient.GetIssuesByStatusChanged(devStatus.JiraStatusCode.ToString(), completeStatus.JiraStatusCode.ToString(), project.Key, initialDate, endDate, 8);
                    if (membersResponse != null)
                    {
                        return membersResponse;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        public async Task<List<SprintDto>> GetSprintsByProjectId(int userId, int projectId)
        {
            try
            {

                List<Sprint> sprints = await _sprintRepository.GetByProjectIdAsync(projectId);
                if (sprints != null)
                    return _mapper.Map<List<SprintDto>>(sprints);

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> ImportSprints(int userId, int projectId)
        {
            try
            {
                int totalImports = 0;
                var project = await _projectRepository.GetByIdAsync(projectId);

                if (project != null && project.JiraDomain.UserId == userId)
                {
                    var apiClient = new AtlassianApiClient(project.JiraDomain.Email, project.JiraDomain.ApiKey, project.JiraDomain.BaseUrl);
                    var sprints = await apiClient.GetSprintsByProjectCode(project.JiraBoardCode);

                    if (sprints != null && sprints.Count > 0)
                    {
                        foreach (var sprint in sprints)
                        {
                            if (await _sprintRepository.GetBySprintCodeAsync(projectId, sprint.id) == null)
                            {
                                Sprint newSprint = new()
                                {
                                    CompleteDate = sprint.completeDate,
                                    ProjectId = projectId,
                                    EndDate = sprint.endDate,
                                    Goal = sprint.goal,
                                    Name = sprint.name,
                                    SprintCode = sprint.id,
                                    StartDate = sprint.startDate,
                                    State = sprint.state
                                };
                                await _sprintRepository.AddAsync(newSprint);
                                if (await _projectRepository.SaveChangesAsync())
                                {
                                    var teamData = await apiClient.GetTeamPerformanceBySprintCode(newSprint.SprintCode, project.JiraBoardCode, newSprint.StartDate, newSprint.CompleteDate ?? newSprint.EndDate, 8);
                                    var membersData = new List<SprintTeamMember>();
                                    foreach (var item in teamData)
                                    {
                                        TeamMember member = await _teamMemberRepository.GetByAccounIdAsync(item.AccountId);
                                        var timeSpanSprint = newSprint.EndDate.Date - newSprint.StartDate.Date;
                                        membersData.Add(new SprintTeamMember()
                                        {
                                            Points = (int)item.Points,
                                            Stories = item.TotalStorys,
                                            TeamMemberId = member.Id,
                                            SprintId = newSprint.Id,
                                            Average = Math.Round((decimal)item.Points / (timeSpanSprint.Days > 0 ? (decimal)timeSpanSprint.Days : (decimal)1),2)
                                        }) ;

                                    }
                                    newSprint.TeamMembers = membersData;
                                    _sprintRepository.Update(newSprint);
                                    totalImports += 1;
                                }
                            }
                        }
                    }

                    if (await _projectRepository.SaveChangesAsync())
                    {
                        return totalImports;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return 0;
        }
        public async Task<List<TeamMemberPerformance>> GetIssuesBySprintId(int userId, int sprintId)
        {
            try
            {
                var sprint = await _sprintRepository.GetByIdAsync(sprintId);
                var project = sprint.Project;
                if (sprint != null && sprint.Project.JiraDomain.UserId == userId)
                {
                    var apiClient = new AtlassianApiClient(project.JiraDomain.Email, project.JiraDomain.ApiKey, project.JiraDomain.BaseUrl);
                    var membersResponse = await apiClient.GetTeamPerformanceBySprintCode(sprint.SprintCode, project.JiraBoardCode, sprint.StartDate, sprint.CompleteDate.HasValue && sprint.CompleteDate.Value != default ? sprint.CompleteDate.Value : sprint.EndDate, 8);
                    if (membersResponse != null)
                    {
                        return membersResponse;
                    }
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
