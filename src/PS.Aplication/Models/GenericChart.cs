using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Aplication.Models
{
    public class GenericChart
    {
        public GenericChart()
        {
            Items = new List<MainItem>();
        }
        public List<MainItem> Items { get; set; }
    }
    
}
