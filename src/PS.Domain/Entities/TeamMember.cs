using PS.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Domain.Entities
{
    [Table("tbTeamMember")]
    public class TeamMember : BaseEntity
    {
        [Column(TypeName = "nvarchar(300)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }
        [Column(TypeName ="decimal(2,2)")]
        public decimal WorkHours { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string JiraAccountId { get; set; }
        public virtual IEnumerable<Project> Projects { get; set; }
        public virtual IEnumerable<SprintTeamMember> Sprints { get; set; }
    }
}
