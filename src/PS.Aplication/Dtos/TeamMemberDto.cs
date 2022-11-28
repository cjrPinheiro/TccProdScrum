using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Aplication.Dtos
{
    public class TeamMemberDto
    {
        public string Name { get; set; }
        public short WorkHours { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
