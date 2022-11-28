using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PS.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserName(this ClaimsPrincipal User) => User.FindFirst(ClaimTypes.Name)?.Value;
        public static int GetUserId(this ClaimsPrincipal User) => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
         
        
    }
}
