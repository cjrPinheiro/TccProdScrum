using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Domain.Entities
{
    [Table("tbSprintTeamMember")]
    public class SprintTeamMember
    {
        public int SprintId { get; set; }
        public int TeamMemberId { get; set; }
        public int Points { get; set; }
        public int Stories { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal Average { get; set; }
        public Sprint Sprint { get; set; }
        public TeamMember TeamMember { get; set; }
    }
}
