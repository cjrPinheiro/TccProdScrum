using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Atlassian.Connector.Models.Requests.TeamMember
{
    public class TeamMemberPerformance
    {
        public string Name { get; set; }
        public double Points { get; set; }
        public decimal DayAverage { get; set; }
        public decimal WorkHours { get; set; }
        public decimal Hours { get; set; }
        public decimal PointsPerHour { get; set; }
        public int TotalStorys { get; set; }
        public string AccountId { get; set; }
    }
}
