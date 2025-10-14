using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Enums
{
    public enum PricingModel
    {
        Flat = 0,
        PerUnit = 1,
        Tiered = 2,
        DimensionBased = 3,
        TimeBased = 4
    }
}
