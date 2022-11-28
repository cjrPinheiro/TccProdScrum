using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Aplication.Dtos
{
    public class JiraDomainEditedDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
    }
}
