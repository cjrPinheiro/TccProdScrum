using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS.Domain.Base;

namespace PS.Domain.Entities
{
    [Table("tbProject")]
    public class Project : BaseEntity
    {
        public int JiraDomainId { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Key { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }
        public int JiraBoardCode { get; set; }
        public int DevelopingStatusId { get; set; }
        public int CompletedStatusId { get; set; }
        public JiraDomain JiraDomain { get; set; }
        public IEnumerable<TeamMember> TeamMembers { get; set; }
        public IEnumerable<Sprint> Sprints { get; set; }
        public virtual IEnumerable<Status> Statuses { get; set; }

    }
}
