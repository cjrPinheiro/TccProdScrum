using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Atlassian.Connector.Models.Requests.TeamMember
{
    public class TeamMemberResponse
    {
        public int maxResults { get; set; }
        public int startAt { get; set; }
        public int total { get; set; }
        public bool isLast { get; set; }
        public List<UserJira> values { get; set; }
    }
}
