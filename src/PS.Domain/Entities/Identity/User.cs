using Microsoft.AspNetCore.Identity;
using PS.Domain.Entities.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PS.Domain.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string LastName { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Description { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string ImageURL { get; set; }
        [Column(TypeName = "smallint")]
        public Function Function { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
        [NotMapped]
        public string FullName
        {
            get 
            {
                string res = string.Empty;
                if(string.IsNullOrWhiteSpace(FirstName) ) 
                    res = $"{this.FirstName} ";
                if (string.IsNullOrWhiteSpace(LastName)) {
                    res = $"{res}{this.LastName}";
                }
                return res;
            }
        }

    }
}
