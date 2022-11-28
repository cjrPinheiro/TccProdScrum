using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Aplication.Models
{
    public class MainItem
    {
        public MainItem()
        {
            Childrens = new List<MainItem>();
        }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public List<MainItem> Childrens { get; set; }
    }
}
