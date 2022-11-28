using Microsoft.Extensions.Options;
using PS.Atlassian.Connector.Interfaces;
using Refit;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using PS.Atlassian.Connector.Models.Requests;
using PS.Atlassian.Connector.Models.Requests.TeamMember;
using PS.Atlassian.Connector.Models.Requests.Issues;
using System.Linq;
using PS.Atlassian.Connector.Models.Requests.Project;
using RestSharp;
using PS.Base.Connector;
using System.Text.Json;
using PS.Atlassian.Connector.Models.Requests.Sprint;
using PS.Atlassian.Connector.Models.Requests.Status;
namespace PS.Atlassian.Connector
{
    public class AtlassianApiClient : BaseApiClient, IAtlassianApiClient
    {
        public AtlassianApiClient(string login, string apiKey, string domainUrl)
        {
            _httpClient = new HttpClient();
            string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                               .GetBytes(login + ":" + apiKey));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);
            _httpClient.BaseAddress = new Uri(domainUrl + "/rest");
            base.SetHttpClient(_httpClient);

        }

        public async Task<ProjectResponse[]> GetProjectsAsync()
        {
            ProjectResponse[] response = null;
            try
            {
                var res = await GetAsync("/api/3/project");
                string stringres = await res.Content.ReadAsStringAsync();
                response = JsonSerializer.Deserialize<ProjectResponse[]>(stringres);

            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        public async Task<TeamMemberResponse> GetTeamMembersAsync(string projectKey)
        {
            TeamMemberResponse response = null;
            try
            {
                Dictionary<string, string> @params = new Dictionary<string, string>() { { "query", "is assignee of " + projectKey } };
                var res = await GetAsync("/api/3/user/search/query", @params);
                string stringres = await res.Content.ReadAsStringAsync();

                response = JsonSerializer.Deserialize<TeamMemberResponse>(stringres);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        public async Task<List<TeamMemberPerformance>> GetIssuesByStatusChanged(string developingStatus, string completedStatus, string projectKey, DateTime initialDate, DateTime endDate, int workHours)
        {
            List<TeamMemberPerformance> result = null;
            try
            {
                string jql = "status changed DURING (#INITALDATE#,#ENDDATE#)  from #INTIALSTATUS# to #ENDSTATUS#  AND project = \"#PRJKEY#\"";

                jql = jql.Replace("#INITALDATE#", initialDate.Date.ToString("yyyy-MM-dd")).Replace("#ENDDATE#", endDate.Date.ToString("yyyy-MM-dd"))
                         .Replace("#INTIALSTATUS#", developingStatus).Replace("#ENDSTATUS#", completedStatus).Replace("#PRJKEY#", projectKey);

                //jql = "status changed DURING (2022-03-05,2022-03-19)  from \"Code Review\" to \"Dev OK\"";

                var client = new RestClient($"{_httpClient.BaseAddress}/api/3/search?jql={jql}");
                var request = new RestRequest();
                request.AddHeader("Authorization", "Basic " + _httpClient.DefaultRequestHeaders.Authorization.Parameter);
                var response = await client.GetAsync(request);

                string stringres = response.Content;

                var responseObj = JsonSerializer.Deserialize<IssueReponse>(stringres);


                if (responseObj != null)
                {
                    TimeSpan date = endDate - initialDate;
                    int timespan = BusinessDaysUntil(initialDate.Date, endDate.Date);

                    result = BuildTeamMemberPoints(responseObj, timespan, workHours);


                }

            }
            catch (Exception e)
            {


            }
            return result;
        }

        private List<TeamMemberPerformance> BuildTeamMemberPoints(IssueReponse response, int timeElapsed, int workHours)
        {
            if (workHours <= 0)
                workHours = 8;

            List<TeamMemberPerformance> res = new List<TeamMemberPerformance>();
            timeElapsed = timeElapsed == 0 ? 1 : timeElapsed;
            if (response.total > 0)
            {
                res = new List<TeamMemberPerformance>();
                foreach (var item in response.issues)
                {
                    if (item.fields.assignee != null)
                    {
                        if (res.Where(q => q.Name == item.fields.assignee.displayName).Any())
                        {
                            res.First(q => q.Name == item.fields.assignee.displayName).Points += item.fields.customfield_10016.HasValue ? item.fields.customfield_10016.Value : 0;
                            res.First(q => q.Name == item.fields.assignee.displayName).TotalStorys += 1;

                        }
                        else
                        {
                            res.Add(new TeamMemberPerformance()
                            {
                                Name = item.fields.assignee != null ? item.fields.assignee.displayName : "Não atribuído",
                                Points = item.fields.customfield_10016.HasValue ? item.fields.customfield_10016.Value : 0,
                                WorkHours = workHours,
                                TotalStorys = 1,
                                AccountId = item.fields.assignee != null ? item.fields.assignee.accountId : "",
                            });
                        }
                    }
                }
                res.ForEach(q => q.DayAverage = Math.Round((decimal)(q.Points / timeElapsed), 2));
                res.ForEach(q => q.PointsPerHour = Math.Round((decimal)(q.DayAverage / q.WorkHours), 4));
                res.ForEach(q => q.Hours = Math.Round((decimal)q.Points / (q.DayAverage / q.WorkHours), 2));
                //double average = someDoubles.Average();
                //double sumOfSquaresOfDifferences = someDoubles.Select(val => (val - average) * (val - average)).Sum();
                //double sd = Math.Sqrt(sumOfSquaresOfDifferences / someDoubles.Length);
            }

            return res;
        }


        public async Task<List<Sprint>> GetSprintsByProjectCode(int projectCode)
        {
            SprintsResponse response = null;
            try
            {
                int projectId = GetProjectId(projectCode);
                var res = await GetAsync($"/agile/1.0/board/{projectId}/sprint");
                string stringres = await res.Content.ReadAsStringAsync();

                response = JsonSerializer.Deserialize<SprintsResponse>(stringres);
                if (response != null && response.maxResults > 0)
                    return response.values;
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }
        public async Task<List<TeamMemberPerformance>> GetTeamPerformanceBySprintCode(int sprintCode, int projectCode, DateTime initialDate, DateTime endDate, int workHours)
        {
            List<TeamMemberPerformance> result = null;
            try
            {
                int projectId = GetProjectId(projectCode);

                var res = await GetAsync($"/agile/1.0/board/{projectId}/sprint/{sprintCode}/issue");
                string stringres = await res.Content.ReadAsStringAsync();

                var responseObj = JsonSerializer.Deserialize<IssueReponse>(stringres);

                if (responseObj != null)
                {
                    if (endDate == default)
                        endDate = DateTime.Now;
                    TimeSpan date = endDate.Date - initialDate.Date;
                    int businessDays = BusinessDaysUntil(initialDate.Date, endDate.Date);

                    result = BuildTeamMemberPoints(responseObj, businessDays, workHours);
                }
            }
            catch (Exception)
            {
                throw;
            }


            return result;
        }
        public async Task<StatusesResponse[]> GetAllStatusesAsync()
        {
            StatusesResponse[] response = null;
            try
            {
                var res = await GetAsync("/api/3/status");
                string stringres = await res.Content.ReadAsStringAsync();

                response = JsonSerializer.Deserialize<StatusesResponse[]>(stringres);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
        private int GetProjectId(int projectCode)
        {
            return projectCode - 9999;
        }
        /// <summary>
        /// Calculates number of business days, taking into account:
        ///  - weekends (Saturdays and Sundays)
        ///  - bank holidays in the middle of the week
        /// </summary>
        /// <param name="firstDay">First day in the time interval</param>
        /// <param name="lastDay">Last day in the time interval</param>
        /// <param name="holidays">List of bank holidays excluding weekends</param>
        /// <returns>Number of business days during the 'span'</returns>
        private int BusinessDaysUntil(DateTime firstDay, DateTime lastDay, params DateTime[] holidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = firstDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)firstDay.DayOfWeek;
                int lastDayOfWeek = lastDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in holidays)
            {
                DateTime bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                    --businessDays;
            }

            return businessDays;
        }


    }
}
