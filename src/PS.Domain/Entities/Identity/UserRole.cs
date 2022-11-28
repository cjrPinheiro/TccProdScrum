using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Domain.Entities.Identity
{
    public class UserRole : IdentityUserRole<int>
    {
        public Role Role  { get; set; }
        public User User { get; set; }

    }
}
