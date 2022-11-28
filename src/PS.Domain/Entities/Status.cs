using PS.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace PS.Domain.Entities
{
    [Table("tbStatus")]
    public class Status : BaseEntity
    {
        public int ProjectId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }
        public int JiraStatusCode { get; set; }
        public virtual Project Project { get; set; }
    }
}
