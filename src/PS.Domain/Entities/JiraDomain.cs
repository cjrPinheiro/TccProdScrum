using PS.Domain.Base;
using PS.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Domain.Entities
{
    [Table("tbJiraDomain")]
    public class JiraDomain : BaseEntity
    {
        [Column(TypeName = "nvarchar(200)")]
        public string Email { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string ApiKey { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string BaseUrl { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<Project> Projects { get; set; }
    }
}
