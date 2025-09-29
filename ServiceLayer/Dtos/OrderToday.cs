using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos
{
    public class OrderToday
    {
        public int Total { get; set; }

        public double GrowthRate { get; set; }
    }
}
