using PS.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Domain.Entities
{
    [Table("tbSprint")]
    public class Sprint : BaseEntity
    {
        public int ProjectId { get; set; }
        public int SprintCode { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string State { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Goal { get; set; }

        public virtual Project Project { get; set; }
        public virtual IEnumerable<SprintTeamMember> TeamMembers { get; set; }
    }
}
