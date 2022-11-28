using PS.Aplication.Interfaces;
using PS.Aplication.Models;
using PS.Domain.Entities;
using PS.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Aplication.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ISprintPersist _sprintRepository;
        private readonly IProjectPersist _projectRepository;
        private readonly ITeamMemberPersist _memberRepository;
        private readonly IJiraService _jiraService;
        public DashboardService(ISprintPersist sprintPersist, IProjectPersist projectPersist, IJiraService jiraService, ITeamMemberPersist memberPersist)
        {
            _sprintRepository = sprintPersist;
            _projectRepository = projectPersist;
            _jiraService = jiraService;
            _memberRepository = memberPersist;
        }
        public async Task<GenericChart> GenProjectsOverviewChart(int userId, int jiraDomainid, DateTime initialDate, DateTime endDate)
        {
            GenericChart chart = null;
            try
            {
                chart = new();
                List<Project> projects = await _projectRepository.GetByDomainIdAsync(jiraDomainid);
                foreach (var prj in projects)
                {
                    MainItem newItem = new()
                    {
                        Description = prj.Name
                    };
                    try
                    {
                        var issues = await _jiraService.GetIssuesByDoneStatuses(userId, prj.Id, initialDate, endDate);
                        if (issues != null && issues.Count > 0)
                        {
                            foreach (var item in issues)
                            {
                                if (newItem.Childrens.Where(q => q.Description == item.Name).Any())
                                {
                                    newItem.Childrens.Where(q => q.Description == item.Name)
                                        .FirstOrDefault().Value += (decimal)item.Points;
                                }
                                else
                                {
                                    newItem.Childrens.Add(new MainItem()
                                    {
                                        Description = item.Name,
                                        Value = (decimal)item.Points
                                    });
                                }
                            }
                            newItem.Value = newItem.Childrens.Sum(q => q.Value);
                            chart.Items.Add(newItem);
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }
                return chart;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<GenericChart> GenMemberAvgHistoryChart(int memberId, int projectId, DateTime initialDate, DateTime endDate)
        {
            GenericChart chart = null;
            try
            {
                chart = new();
                var member = await _memberRepository.GetByIdAndProjectIdAsync(memberId, projectId, initialDate, endDate);
                if (member != null)
                {

                    foreach (var sprint in member.Sprints)
                    {
                        MainItem newItem = new()
                        {
                            Description = sprint.Sprint.StartDate.ToString("dd/MM/yy"),
                            Value = sprint.Average
                        };
                        chart.Items.Add(newItem);
                    }
                }
                return chart;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
