using PresentationLayer.Areas.Stores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Abstractions.IServices
{
    public interface IDashboardService
    {
        public Task<DashboardDto> GetDashboard(Guid storeId);
    }
}
