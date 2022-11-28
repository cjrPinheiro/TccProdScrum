using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Application.Dtos
{
    public class UserExistingDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Function { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }

    }
}
