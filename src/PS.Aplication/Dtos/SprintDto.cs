using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Aplication.Dtos
{
    public class SprintDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int SprintCode { get; set; }
        public string State { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CompleteDate { get; set; }
        public string Goal { get; set; }
    }
}
