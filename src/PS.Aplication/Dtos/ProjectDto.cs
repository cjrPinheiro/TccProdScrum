using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Aplication.Dtos
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public int JiraBoardCode { get; set; }
        public int DevelopingStatusId { get; set; }
        public string DevelopingStatus { get; set; }
        public int CompletedStatusId { get; set; }
        public string CompletedStatus { get; set; }
        public List<StatusDto> Statuses { get; set; }
        //public StatusDto DevelopingStatus { get; set; }
        //public StatusDto CompletedStatus { get; set; }
    }
}
